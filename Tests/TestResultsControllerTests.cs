using Api.Controllers;
using Application.DTOs.Tests;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Tests
{
    public class TestResultsControllerTests
    {
        private readonly Mock<ITestResultRepository> _mockTestResultRepo = new();
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly IMapper _mapper;

        private readonly DefaultHttpContext _httpContext;

        public TestResultsControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            var cfg = new MapperConfiguration(c => c.AddProfile<AutoMapperProfile>());
            _mapper = cfg.CreateMapper();

            _httpContext = new DefaultHttpContext();
        }

        private TestResultsController CreateController(string userId)
        {
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            var controller = new TestResultsController(_mockTestResultRepo.Object, _mockUserManager.Object, _mapper);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext
            };

            _httpContext.User = new ClaimsPrincipal(
                new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                })
            );

            return controller;
        }

        [Fact]
        public async Task GetMyResults_ReturnsCorrectResults()
        {
            // Arrange
            string userId = "student123";
            var expectedResults = new List<TestResult>
            {
                new() { Id = 1, UserId = userId, Score = 8, TotalQuestions = 10, CompletedDate = DateTime.UtcNow, Test = new Test { Title = "Sample Test" } }
            };

            _mockTestResultRepo
              .Setup(r => r.FindAsync(
                  It.IsAny<Expression<Func<TestResult, bool>>>(),
                  It.IsAny<Func<IQueryable<TestResult>, IQueryable<TestResult>>>()))
              .ReturnsAsync(expectedResults);

            var controller = CreateController(userId);

            // Act
            var result = await controller.GetMyResults();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var list = Assert.IsType<List<TestResultDto>>(okResult.Value);
            Assert.Single(list);
            Assert.Equal("Sample Test", list[0].TestTitle);
        }

        [Fact]
        public async Task GetResultDetail_ReturnsNotFound_IfWrongUser()
        {
            // Arrange
            string userId = "student123";
            var testResults = new List<TestResult>
            {
                new() { Id = 5, UserId = "otherStudent", Test = new Test { Title = "Secret Test" } }
            };

            _mockTestResultRepo
              .Setup(r => r.FindAsync(It.IsAny<Expression<Func<TestResult, bool>>>()))
              .ReturnsAsync((Expression<Func<TestResult, bool>> predicate) =>
                  testResults.Where(predicate.Compile()).ToList()
              );

            var controller = CreateController(userId);

            // Act
            var result = await controller.GetResultDetail(5);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetResultDetail_ReturnsCorrectResult()
        {
            // Arrange
            string userId = "student123";
            int testResultId = 42;

            var testResult = new TestResult
            {
                Id = testResultId,
                UserId = userId,
                Score = 7,
                TotalQuestions = 10,
                CompletedDate = DateTime.UtcNow,
                Test = new Test { Title = "Unit Testing FTW" }
            };

            var testResults = new List<TestResult> { testResult };

            _mockTestResultRepo
              .Setup(r => r.FindAsync(
                  It.IsAny<Expression<Func<TestResult, bool>>>(),
                  It.IsAny<Func<IQueryable<TestResult>, IQueryable<TestResult>>>()))
              .ReturnsAsync(new[] { testResult });

            var controller = CreateController(userId);

            // Act
            var result = await controller.GetResultDetail(testResultId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<TestResultDto>(okResult.Value);
            Assert.Equal("Unit Testing FTW", dto.TestTitle);
            Assert.Equal(7, dto.Score);
        }

        [Fact]
        public async Task GetResultDetail_ReturnsNotFound_IfResultMissing()
        {
            // Arrange
            string userId = "student123";
            int missingId = 99;

            _mockTestResultRepo.Setup(r => r.FindAsync(
                    It.IsAny<Expression<Func<TestResult, bool>>>(),
                    It.IsAny<Func<IQueryable<TestResult>, IQueryable<TestResult>>?>()))
                .ReturnsAsync(new List<TestResult>());

            var controller = CreateController(userId);

            // Act
            var result = await controller.GetResultDetail(missingId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetResultDetail_ReturnsNotFound_IfResultBelongsToOtherUser()
        {
            // Arrange
            var userId = "student123";
            var testResultId = 123;

            _mockTestResultRepo
              .Setup(r => r.FindAsync(
                  It.IsAny<Expression<Func<TestResult, bool>>>(),
                  It.IsAny<Func<IQueryable<TestResult>, IQueryable<TestResult>>>()))
              .ReturnsAsync(new List<TestResult>());

            var controller = CreateController(userId);

            // Act
            var result = await controller.GetResultDetail(testResultId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task SubmitTest_ReturnsScore_WhenCorrectAnswersSubmitted()
        {
            // Arrange
            var userId = "student123";
            var testId = 1;

            var test = new Test
            {
                Id = testId,
                Title = "Mock Test",
                Questions = new List<Question>
                {
                    new()
                    {
                        Id = 100,
                        Content = "Q1",
                        Options = new List<Option>
                        {
                            new() { Id = 1, Text = "A", IsCorrect = true },
                            new() { Id = 2, Text = "B", IsCorrect = false }
                        }
                    },
                    new()
                    {
                        Id = 101,
                        Content = "Q2",
                        Options = new List<Option>
                        {
                            new() { Id = 3, Text = "C", IsCorrect = false },
                            new() { Id = 4, Text = "D", IsCorrect = true }
                        }
                    }
                }
            };

            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _mockTestResultRepo.Setup(r => r.GetTestWithQuestionsAsync(testId))
                .ReturnsAsync(test);

            _mockTestResultRepo.Setup(r => r.SaveTestResultAsync(It.IsAny<TestResult>()))
                .Returns(Task.CompletedTask);

            var controller = CreateControllerWithRepo(userId);

            var submission = CreateMockSubmission(testId, new Dictionary<int, int>
            {
                { 100, 1 },
                { 101, 4 } 
            });

            // Act
            var result = await controller.SubmitTest(submission);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<TestResultDto>(ok.Value);
            Assert.Equal(2, dto.Score);
            Assert.Equal(2, dto.TotalQuestions);
        }

        [Fact]
        public async Task SubmitTest_ReturnsNotFound_WhenTestDoesNotExist()
        {
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("student123");

            _mockTestResultRepo.Setup(r => r.GetTestWithQuestionsAsync(It.IsAny<int>()))
                .ReturnsAsync((Test?)null);

            var controller = CreateControllerWithRepo("student123");

            var submission = new TestSubmissionDto { TestId = 99 };

            var result = await controller.SubmitTest(submission);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task SubmitTest_ReturnsUnauthorized_IfUserNotLoggedIn()
        {
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns((string?)null);

            var controller = CreateControllerWithRepo(null);

            var submission = new TestSubmissionDto { TestId = 1 };

            var result = await controller.SubmitTest(submission);

            Assert.IsType<UnauthorizedResult>(result);
        }


        private static TestSubmissionDto CreateMockSubmission(int testId, Dictionary<int, int> questionAnswers)
        {
            return new TestSubmissionDto
            {
                TestId = testId,
                Answers = questionAnswers.Select(kvp => new AnswerSubmissionDto
                {
                    QuestionId = kvp.Key,
                    SelectedOptionId = kvp.Value
                }).ToList()
            };
        }

        private TestResultsController CreateControllerWithRepo(string? userId)
        {
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            var controller = new TestResultsController(_mockTestResultRepo.Object, _mockUserManager.Object, _mapper);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = userId != null
                        ? new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) }))
                        : new ClaimsPrincipal(new ClaimsIdentity())
                }
            };
            return controller;
        }

    }
}
