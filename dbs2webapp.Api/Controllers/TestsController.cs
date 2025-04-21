using dbs2webapp.Application.DTOs.Tests;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TestsController : ControllerBase
    {
        private readonly IBaseRepository<Test> _testRepo;
        private readonly IBaseRepository<Chapter> _chapterRepo;
        private readonly IMapper _mapper;

        public TestsController(
            IBaseRepository<Test> testRepo,
            IBaseRepository<Chapter> chapterRepo,
            IMapper mapper)
        {
            _testRepo = testRepo;
            _chapterRepo = chapterRepo;
            _mapper = mapper;
        }

        // POST: api/tests
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost("{chapterId}")]
        public async Task<IActionResult> Create(int chapterId, [FromBody] CreateTestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var chapters = await _chapterRepo.FindAsync(
                c => c.Id == chapterId,
                include: q => q.Include(c => c.Course)
            );

            var chapter = chapters.SingleOrDefault();
            if (chapter == null)
                return NotFound("Chapter not found");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (chapter.Course!.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            if (!ValidateCorrectOptionIndexes(dto))
                return BadRequest("Invalid CorrectOptionIndex for one or more questions.");

            var test = _mapper.Map<Test>(dto);
            test.ChapterId = chapterId;
            await _testRepo.AddAsync(test);
            await _testRepo.SaveAsync();

            return CreatedAtAction(nameof(GetById), new { id = test.Id }, _mapper.Map<TestDto>(test));
        }

        // PUT: api/tests/{id}
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateTestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var testQuery = await _testRepo.FindAsync(
                t => t.Id == id,
                include: q => q
                    .Include(t => t.Chapter!).ThenInclude(ch => ch.Course)
                    .Include(t => t.Questions!).ThenInclude(q => q.Options));

            var test = testQuery.FirstOrDefault();
            if (test == null)
                return NotFound("Test not found.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (test.Chapter?.Course?.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            if (!ValidateCorrectOptionIndexes(dto))
                return BadRequest("Invalid CorrectOptionIndex for one or more questions.");

            // Clear old questions
            test.Questions?.Clear();

            // Update core fields
            test.Title = dto.Title;
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

        // GET: api/tests/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var test = await _testRepo.FindAsync(
                t => t.Id == id,
                include: q => q.Include(t => t.Questions!).ThenInclude(q => q.Options));

            var entity = test.FirstOrDefault();
            return entity == null
                ? NotFound()
                : Ok(_mapper.Map<TestDto>(entity));
        }

        // GET: /api/chapters/{chapterId}/tests
        [HttpGet("/api/chapters/{chapterId}/tests")]
        [Authorize]
        public async Task<IActionResult> GetTestsForChapter(int chapterId)
        {
            var tests = await _testRepo.FindAsync(
                t => t.ChapterId == chapterId,
                include: q => q.Include(t => t.Questions!).ThenInclude(q => q.Options));

            return Ok(_mapper.Map<List<TestDto>>(tests));
        }

        // DELETE: api/tests/{id}
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

        // Internal helper
        private bool ValidateCorrectOptionIndexes(CreateTestDto dto)
        {
            return dto.Questions.All(q =>
                q.CorrectOptionIndex >= 0 &&
                q.CorrectOptionIndex < q.Options.Count);
        }
    }
}