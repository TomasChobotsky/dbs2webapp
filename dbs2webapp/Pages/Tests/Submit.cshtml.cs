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

        public async Task<IActionResult> OnPostAsync(int chapterId, Dictionary<int, int> selectedOptions)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Get chapter and test
            Chapter = await _context.Chapters
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == chapterId);

            if (Chapter == null)
            {
                return NotFound();
            }

            Test = await _context.Tests
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(t => t.ChapterId == chapterId);

            if (Test == null)
            {
                return NotFound();
            }

            // Calculate score
            TotalQuestions = Test.Questions.Count;
            Score = 0;
            UserSelections = selectedOptions ?? new Dictionary<int, int>();

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

            // Save test result
            var testResult = new TestResult
            {
                UserId = user.Id,
                TestId = Test.Id,
                Score = Score,
                TotalQuestions = TotalQuestions
            };

            try
            {
                _context.TestResults.Add(testResult);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ModelState.AddModelError("", "Error saving test results.");
                return Page();
            }

            return Page();
        }
    }
}