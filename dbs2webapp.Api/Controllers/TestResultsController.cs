using dbs2webapp.Application.DTOs.Tests;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class TestResultsController : ControllerBase
    {
        private readonly ITestResultRepository _testResultRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public TestResultsController(
            ITestResultRepository testResultRepo,
            UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _testResultRepo = testResultRepo;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: api/testresults/my
        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<TestResultDto>>> GetMyResults()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // we’re pulling in the Test navigation, so the delegate returns IIncludableQueryable<…,Test>
            var results = await _testResultRepo.FindAsync(
                tr => tr.UserId == userId,
                include: q => q.Include(r => r.Test)
            );

            var dtos = _mapper.Map<List<TestResultDto>>(results);
            return Ok(dtos);
        }

        // GET: api/testresults/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TestResultDto>> GetResultDetail(int id)
        {
            var userId = _userManager.GetUserId(User);
                if (userId == null) return Unauthorized();

            var result = await _testResultRepo.FindAsync(
                r => r.Id == id && r.UserId == userId,
                include: q => q.Include(r => r.Test));

            var entity = result.FirstOrDefault();
            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<TestResultDto>(entity));
        }

        // POST: api/testresults
        [HttpPost]
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

            return Ok(_mapper.Map<TestResultDto>(result));
        }
    }
}
