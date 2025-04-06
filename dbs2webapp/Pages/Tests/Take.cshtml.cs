using dbs2webapp.Data;
using dbs2webapp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> OnGetAsync(int chapterId)
        {
            // Get the chapter with its test and questions
            Chapter = await _context.Chapters
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == chapterId);

            if (Chapter == null)
            {
                return NotFound();
            }

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

            // Get the test with questions and options
            Test = await _context.Tests
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(t => t.ChapterId == chapterId);

            if (Test == null)
            {
                TempData["InfoMessage"] = "No test available for this chapter yet.";
                return RedirectToPage("/Chapters/Index", new { courseId = Chapter.CourseId });
            }

            Questions = Test.Questions.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int chapterId, Dictionary<int, int> selectedOptions)
        {
            // We'll implement test submission in the next step
            return RedirectToPage("/Tests/Submit", new { chapterId });
        }
    }
}