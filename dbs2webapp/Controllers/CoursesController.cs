using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly IBaseRepository<Course> _repo;
        private readonly UserManager<IdentityUser> _userManager;

        public CoursesController(IBaseRepository<Course> repo, UserManager<IdentityUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        // GET: api/courses
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _repo.GetAllAsync();
            return Ok(courses);
        }

        // GET: api/courses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _repo
            .FindAsync(c => c.Id == id, include: q => q
                .Include(c => c.Chapters!)
                    .ThenInclude(ch => ch.Tests));

            var result = course.FirstOrDefault();
            return result == null ? NotFound() : Ok(result);
        }

        // GET: api/courses/mine
        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyCourses()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var courses = await _repo.FindAsync(c => c.TeacherId == userId,
                include: q => q.Include(c => c.Chapters!).ThenInclude(ch => ch.Tests));
            return Ok(courses);
        }

        // POST: api/courses
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Course course)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            course.TeacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            course.CreatedDate = DateTime.UtcNow;

            await _repo.AddAsync(course);
            await _repo.SaveAsync();

            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        // PUT: api/courses/{id}
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Course updated)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (course.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            course.Name = updated.Name;
            course.Description = updated.Description;
            await _repo.SaveAsync();

            return NoContent();
        }

        // DELETE: api/courses/{id}
        [Authorize(Roles = "Teacher,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (course.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            _repo.Remove(course);
            await _repo.SaveAsync();
            return NoContent();
        }
    }
}