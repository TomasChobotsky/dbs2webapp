using dbs2webapp.Application.DTOs.Tests;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dbs2webapp.Application.DTOs.Tests.Result;
using dbs2webapp.Domain.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class TestResultsController : ControllerBase
    {
        private readonly IBaseRepository<TestResult> _testResultRepo;
        private readonly IBaseRepository<Test> _testRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public TestResultsController(
            IBaseRepository<TestResult> testResultRepo,
            IBaseRepository<Test> testRepo,
            UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _testResultRepo = testResultRepo;
            _testRepo = testRepo;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<int>> Submit(TestSubmissionDto submission)
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null) return Unauthorized();

            // 1) Load the test with options to know what's correct
            var test = (await _testRepo.FindAsync(
                            t => t.Id == submission.TestId,
                            q => q.Include(t => t.Questions)
                                  .ThenInclude(qq => qq.Options)))
                       .FirstOrDefault();

            if (test is null) return NotFound("Test not found.");

            // 2) Build answer entities + compute score
            var answers = new List<TestAnswer>();
            int score = 0;

            foreach (var ans in submission.Answers)
            {
                answers.Add(new TestAnswer
                {
                    QuestionId = ans.QuestionId,
                    SelectedOptionId = ans.SelectedOptionId
                });

                // is the chosen option correct?
                var correct = test.Questions
                                  .SelectMany(q => q.Options)
                                  .Any(o => o.Id == ans.SelectedOptionId && o.IsCorrect);
                if (correct) score++;
            }

            // 3) Create result
            var result = new TestResult
            {
                UserId = userId,
                TestId = test.Id,
                CompletedDate = DateTime.UtcNow,
                TotalQuestions = submission.Answers.Count,
                Score = score,
                Answers = answers
            };

            await _testResultRepo.AddAsync(result);
            await _testResultRepo.SaveAsync();

            return Ok(result.Id);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<TestResultDetailsDto>> GetResultDetail(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null) return Unauthorized();

            var entity = (await _testResultRepo.FindAsync(
                            r => r.Id == id && r.UserId == userId,
                            q => q
                                .Include(r => r.Answers)
                                .Include(r => r.Test)
                                    .ThenInclude(t => t.Questions)
                                        .ThenInclude(qq => qq.Options)))
                         .FirstOrDefault();

            if (entity is null) return NotFound();

            var dto = _mapper.Map<TestResultDetailsDto>(
                entity,
                opt => opt.Items["ChosenOptionIds"] =
                           entity.Answers
                                 .Select(a => a.SelectedOptionId)
                                 .ToHashSet());

            return Ok(dto);
        }

        // GET api/testresults
        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<TestResultDto>>> ListMine()
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null) return Unauthorized();

            var results = await _testResultRepo.FindAsync(
                r => r.UserId == userId,
                include: q => q.Include(r => r.Test));

            return Ok(_mapper.Map<IEnumerable<TestResultDto>>(results
                     .OrderByDescending(r => r.CompletedDate)));
        }
    }
}
