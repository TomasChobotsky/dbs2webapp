using Application.DTOs.Tests;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class TestResultsController : ControllerBase
    {
        private readonly IBaseRepository<TestResult> _repository;
        private readonly ITestResultRepository _testResultRepo;
        private readonly UserManager<IdentityUser> _userManager;

        public TestResultsController(IBaseRepository<TestResult> repository, ITestResultRepository testResultRepo, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _testResultRepo = testResultRepo;
            _userManager = userManager;
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<TestResultDto>>> GetMyResults()
        {
            var userId = _userManager.GetUserId(User);

            var results = await _repository.FindAsync(tr => tr.UserId == userId);
            var dtos = results.Select(r => new TestResultDto
            {
                Id = r.Id,
                TestTitle = r.Test!.Title ?? "(Unnamed Test)",
                Score = r.Score,
                TotalQuestions = r.TotalQuestions,
                CompletedDate = r.CompletedDate
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestResultDto>> GetResultDetail(int id)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _repository
                .FindAsync(r => r.Id == id && r.UserId == userId);

            var entity = result.FirstOrDefault();
            if (entity == null)
                return NotFound();

            var dto = new TestResultDto
            {
                Id = entity.Id,
                TestTitle = entity.Test?.Title ?? "(Unknown Test)",
                Score = entity.Score,
                TotalQuestions = entity.TotalQuestions,
                CompletedDate = entity.CompletedDate
            };

            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitTest([FromBody] TestSubmissionDto submission)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var test = await _testResultRepo.GetTestWithQuestionsAsync(submission.TestId);
            if (test == null)
                return NotFound($"Test with id {submission.TestId} not found.");

            int correct = 0;
            int total = test.Questions!.Count;

            foreach (var q in test.Questions)
            {
                var submitted = submission.Answers.FirstOrDefault(a => a.QuestionId == q.Id);
                if (submitted == null) continue;

                var correctOption = q.Options!.FirstOrDefault(o => o.IsCorrect);
                if (correctOption != null && submitted.SelectedOptionId == correctOption.Id)
                    correct++;
            }

            var result = new TestResult
            {
                UserId = userId,
                TestId = test.Id,
                Score = correct,
                TotalQuestions = total,
                CompletedDate = DateTime.UtcNow
            };

            await _testResultRepo.SaveTestResultAsync(result);

            return Ok(new TestResultDto
            {
                Id = result.Id,
                TestTitle = test.Title ?? "(Unnamed Test)",
                Score = correct,
                TotalQuestions = total,
                CompletedDate = result.CompletedDate
            });
        }
    }
}
