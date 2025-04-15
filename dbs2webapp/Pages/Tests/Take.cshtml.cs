using dbs2webapp.Data;
using dbs2webapp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dbs2webapp.Pages.Tests
{
    public class TakeModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TakeModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Chapter Chapter { get; set; }
        public Test Test { get; set; }
        public List<Question> Questions { get; set; }

        // Now we expect a testId instead of a chapterId
        public async Task<IActionResult> OnGetAsync(int testId)
        {
            // Load the test with its questions, options, and associated Chapter & Course
            Test = await _context.Tests
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Options)
                .Include(t => t.Chapter)
                    .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(t => t.Id == testId);

            if (Test == null)
            {
                TempData["InfoMessage"] = "No test available for this chapter yet.";
                // Redirect to the chapters list page using a fallback course id, if appropriate,
                // or simply return NotFound() if you prefer.
                return RedirectToPage("/Chapters/Index");
            }

            // Retrieve the chapter from the test
            Chapter = Test.Chapter;

            // Check if user is enrolled in the course
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var isEnrolled = await _context.UserCourses
                    .AnyAsync(uc => uc.UserId == user.Id && uc.CourseId == Chapter.CourseId);

                if (!isEnrolled)
                {
                    TempData["ErrorMessage"] = "You must enroll in this course first!";
                    return RedirectToPage("/Courses/Index");
                }
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Prepare the list of questions to display
            Questions = Test.Questions.ToList();
            return Page();
        }

        // Update the OnPostAsync method so it also accepts testId rather than chapterId.
        public async Task<IActionResult> OnPostAsync(int testId, Dictionary<int, int> selectedOptions)
        {
            // We'll implement test submission in the next step.
            // For now, simply pass the testId along to the submission page.
            return RedirectToPage("/Tests/Submit", new { testId });
        }
    }
}
