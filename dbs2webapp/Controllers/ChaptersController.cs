using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChaptersController : ControllerBase
    {
        private readonly IBaseRepository<Chapter> _chapterRepo;
        private readonly IBaseRepository<Course> _courseRepo;
        private readonly UserManager<IdentityUser> _userManager;

        public ChaptersController(
            IBaseRepository<Chapter> chapterRepo,
            IBaseRepository<Course> courseRepo,
            UserManager<IdentityUser> userManager)
        {
            _chapterRepo = chapterRepo;
            _courseRepo = courseRepo;
            _userManager = userManager;
        }

        // GET: api/chapters/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var chapter = await _chapterRepo.FindAsync(
                c => c.Id == id,
                include: q => q.Include(ch => ch.Course).Include(ch => ch.Tests));

            var result = chapter.FirstOrDefault();
            return result == null ? NotFound() : Ok(result);
        }

        // GET: api/chapters/by-course/{courseId}
        [HttpGet("by-course/{courseId}")]
        public async Task<IActionResult> GetByCourse(int courseId)
        {
            var chapters = await _chapterRepo.FindAsync(
                c => c.CourseId == courseId,
                include: q => q.Include(ch => ch.Tests));

            return Ok(chapters.OrderBy(c => c.Order));
        }

        // POST: api/chapters/{courseId}
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost("{courseId}")]
        public async Task<IActionResult> Create(int courseId, [FromBody] Chapter input)
        {
            var course = await _courseRepo.GetByIdAsync(courseId);
            if (course == null) return NotFound("Course not found");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (course.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            var maxOrder = (await _chapterRepo.FindAsync(c => c.CourseId == courseId))
                            .Max(c => (int?)c.Order) ?? 0;

            var chapter = new Chapter
            {
                Name = input.Name,
                Description = input.Description,
                Content = input.Content,
                CreatedDate = DateTime.UtcNow,
                CourseId = courseId,
                Order = maxOrder + 1
            };

            await _chapterRepo.AddAsync(chapter);
            await _chapterRepo.SaveAsync();

            return CreatedAtAction(nameof(GetById), new { id = chapter.Id }, chapter);
        }

        // PUT: api/chapters/{id}
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Chapter input)
        {
            var chapter = await _chapterRepo.FindAsync(
                c => c.Id == id,
                include: q => q.Include(ch => ch.Course));

            var existing = chapter.FirstOrDefault();
            if (existing == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (existing.Course!.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            existing.Name = input.Name;
            existing.Description = input.Description;
            existing.Content = input.Content;
            existing.Order = input.Order;

            await _chapterRepo.SaveAsync();
            return NoContent();
        }

        // DELETE: api/chapters/{id}
        [Authorize(Roles = "Teacher,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var chapter = await _chapterRepo.FindAsync(
                c => c.Id == id,
                include: q => q.Include(ch => ch.Course));

            var existing = chapter.FirstOrDefault();
            if (existing == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (existing.Course!.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            _chapterRepo.Remove(existing);
            await _chapterRepo.SaveAsync();

            return NoContent();
        }
    }
}
