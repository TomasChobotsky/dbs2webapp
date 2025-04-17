using Application.DTOs.Tests;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestsController : ControllerBase
    {
        private readonly IBaseRepository<Test> _testRepo;
        private readonly IBaseRepository<Chapter> _chapterRepo;

        public TestsController(
            IBaseRepository<Test> testRepo,
            IBaseRepository<Chapter> chapterRepo)
        {
            _testRepo = testRepo;
            _chapterRepo = chapterRepo;
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var chapter = await _chapterRepo.FindAsync(
                c => c.Id == dto.ChapterId,
                include: q => q.Include(c => c.Course));

            var chapterEntity = chapter.FirstOrDefault();
            if (chapterEntity == null)
                return NotFound("Chapter not found");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (chapterEntity.Course!.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            // Validate all questions have correct option indexes
            foreach (var q in dto.Questions)
            {
                if (q.CorrectOptionIndex < 0 || q.CorrectOptionIndex >= q.Options.Count)
                    return BadRequest("Invalid CorrectOptionIndex for one or more questions.");
            }

            var test = new Test
            {
                Title = dto.Title,
                ChapterId = dto.ChapterId,
                Questions = dto.Questions.Select(q => new Question
                {
                    Content = q.Content,
                    Options = q.Options.Select((o, i) => new Option
                    {
                        Text = o.Text,
                        IsCorrect = (i == q.CorrectOptionIndex)
                    }).ToList()
                }).ToList()
            };

            await _testRepo.AddAsync(test);
            await _testRepo.SaveAsync();

            return CreatedAtAction(nameof(GetById), new { id = test.Id }, test);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateTestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingTestQuery = await _testRepo.FindAsync(
                t => t.Id == id,
                include: q => q
                    .Include(t => t.Chapter!)
                        .ThenInclude(ch => ch.Course)
                    .Include(t => t.Questions!)
                        .ThenInclude(q => q.Options));

            var test = existingTestQuery.FirstOrDefault();
            if (test == null)
                return NotFound("Test not found.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (test.Chapter?.Course?.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            // Validate input
            foreach (var q in dto.Questions)
            {
                if (q.CorrectOptionIndex < 0 || q.CorrectOptionIndex >= q.Options.Count)
                    return BadRequest("Invalid CorrectOptionIndex for one or more questions.");
            }

            // Update title
            test.Title = dto.Title;

            // Remove old questions & options
            test.Questions?.Clear();

            // Recreate from input
            test.Questions = dto.Questions.Select(q => new Question
            {
                Content = q.Content,
                Options = q.Options.Select((o, i) => new Option
                {
                    Text = o.Text,
                    IsCorrect = (i == q.CorrectOptionIndex)
                }).ToList()
            }).ToList();

            await _testRepo.SaveAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var test = await _testRepo.FindAsync(
                t => t.Id == id,
                include: q => q
                    .Include(t => t.Questions!)
                    .ThenInclude(q => q.Options));

            var result = test.FirstOrDefault();
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("/api/chapters/{chapterId}/tests")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> GetTestsForChapter(int chapterId)
        {
            var tests = await _testRepo.FindAsync(
                t => t.ChapterId == chapterId,
                include: q => q
                    .Include(t => t.Questions!)
                    .ThenInclude(q => q.Options));

            return Ok(tests);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var test = await _testRepo.FindAsync(
                t => t.Id == id,
                include: q => q.Include(t => t.Chapter!).ThenInclude(c => c.Course));

            var existing = test.FirstOrDefault();
            if (existing == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (existing.Chapter?.Course?.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            _testRepo.Remove(existing);
            await _testRepo.SaveAsync();
            return NoContent();
        }
    }

}