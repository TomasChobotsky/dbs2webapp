using dbs2webapp.Data;
using dbs2webapp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace dbs2webapp.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public string? CurrentUserId { get; set; }

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Course> Courses { get; set; }
        public Dictionary<int, bool> UserEnrollments { get; set; } = new Dictionary<int, bool>();
         
        public async Task OnGetAsync()
        {
            CurrentUserId = _userManager.GetUserId(User);

            Courses = await _context.Courses
                .Include(c => c.Chapters)
                .Include(c => c.Teacher)
                .ToListAsync();

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var userCourses = await _context.UserCourses
                    .Where(uc => uc.UserId == user.Id)
                    .ToListAsync();

                foreach (var course in Courses)
                {
                    UserEnrollments[course.Id] = userCourses.Any(uc => uc.CourseId == course.Id);
                }
            }
        }

        public async Task<IActionResult> OnPostEnrollAsync(int courseId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Identity/Account/Login", new { area = "Identity" });
            }

            var user = await _userManager.GetUserAsync(User);
            var existingEnrollment = await _context.UserCourses
                .FirstOrDefaultAsync(uc => uc.UserId == user.Id && uc.CourseId == courseId);

            if (existingEnrollment == null)
            {
                var userCourse = new UserCourse
                {
                    UserId = user.Id,
                    CourseId = courseId
                };

                _context.UserCourses.Add(userCourse);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Successfully enrolled in the course!";
            }
            else
            {
                TempData["InfoMessage"] = "You're already enrolled in this course!";
            }

            return RedirectToPage();
        }
    }
}