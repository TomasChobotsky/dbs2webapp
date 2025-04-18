using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public CoursesController(
            IBaseRepository<Course> repo,
            UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _repo = repo;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: api/courses
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _repo.GetAllAsync();
            var dto = _mapper.Map<List<CourseDto>>(courses);
            return Ok(dto);
        }

        // GET: api/courses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _repo.FindAsync(c => c.Id == id,
                include: q => q.Include(c => c.Chapters!).ThenInclude(ch => ch.Tests));

            var result = course.FirstOrDefault();
            return result == null ? NotFound() : Ok(_mapper.Map<CourseDto>(result));
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

            return Ok(_mapper.Map<List<CourseDto>>(courses));
        }

        // POST: api/courses
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Map<Course>(dto);
            entity.TeacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            entity.CreatedDate = DateTime.UtcNow;

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();

            var resultDto = _mapper.Map<CourseDto>(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, resultDto);
        }

        // PUT: api/courses/{id}
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateCourseDto dto)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (course.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            _mapper.Map(dto, course); // Map updated values onto existing entity
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