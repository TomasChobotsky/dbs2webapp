using dbs2webapp.Data;
using dbs2webapp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dbs2webapp.Pages.Chapters
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Course Course { get; set; }
        public List<Chapter> Chapters { get; set; }
        public bool IsEnrolled { get; set; }

        public async Task<IActionResult> OnGetAsync(int courseId)
        {
            // Get the course with its chapters
            Course = await _context.Courses
                .Include(c => c.Chapters)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (Course == null)
            {
                return NotFound();
            }

            // Check if user is enrolled
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                IsEnrolled = await _context.UserCourses
                    .AnyAsync(uc => uc.UserId == user.Id && uc.CourseId == courseId);
            }

            if (!IsEnrolled && User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You must enroll in this course first!";
                return RedirectToPage("/Courses/Index");
            }

            Chapters = Course.Chapters.ToList();
            return Page();
        }
    }
}