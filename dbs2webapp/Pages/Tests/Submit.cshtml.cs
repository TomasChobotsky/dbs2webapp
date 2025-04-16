using dbs2webapp.Data;
using dbs2webapp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dbs2webapp.Pages.Tests
{
    public class SubmitModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SubmitModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Chapter Chapter { get; set; }
        public Test Test { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public Dictionary<int, Option> CorrectAnswers { get; set; } = new Dictionary<int, Option>();
        public Dictionary<int, int> UserSelections { get; set; } = new Dictionary<int, int>();

        public async Task<IActionResult> OnGetAsync(int chapterId)
        {
            // This page should only be accessed after submission
            return RedirectToPage("/Tests/Take", new { chapterId });
        }

        public async Task<IActionResult> OnPostAsync(int testId, Dictionary<int, int> selectedOptions)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Fetch the Test (and its Chapter) by the test's Id.
            Test = await _context.Tests
                .Include(t => t.Chapter)
                    .ThenInclude(ch => ch.Course)
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(t => t.Id == testId);

            if (Test == null)
            {
                return NotFound();
            }

            // Retrieve the Chapter for display
            Chapter = Test.Chapter;
            if (Chapter == null)
            {
                return NotFound();
            }

            // Make sure we have at least one question
            TotalQuestions = Test.Questions.Count;
            Score = 0;
            UserSelections = selectedOptions ?? new Dictionary<int, int>();

            // Determine which options were correct
            foreach (var question in Test.Questions)
            {
                var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                if (correctOption != null)
                {
                    CorrectAnswers[question.Id] = correctOption;
                    if (UserSelections.TryGetValue(question.Id, out int selectedOptionId))
                    {
                        if (selectedOptionId == correctOption.Id)
                        {
                            Score++;
                        }
                    }
                }
            }

            // Create a new TestResult
            var testResult = new TestResult
            {
                UserId = user.Id,
                TestId = Test.Id,                 // Now uses the correct Test Id
                Score = Score,
                TotalQuestions = TotalQuestions,
                CompletedDate = DateTime.Now      // If you want a timestamp
            };

            try
            {
                _context.TestResults.Add(testResult);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception, show error, etc.
                ModelState.AddModelError("", "Error saving test results.");
                return Page();
            }

            return Page();
        }
    }
}