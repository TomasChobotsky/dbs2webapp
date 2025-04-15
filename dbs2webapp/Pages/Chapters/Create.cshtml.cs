using dbs2webapp.Data;
using dbs2webapp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace dbs2webapp.Pages.Chapters
{
    [Authorize(Roles = "Teacher,Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(ApplicationDbContext context,
                         UserManager<IdentityUser> userManager,
                         IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        public Chapter Chapter { get; set; }

        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Course == null) return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            if (Course.TeacherId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Course = await _context.Courses.FindAsync(Course.Id);
                return Page();
            }

            // Set basic chapter info
            Chapter.CourseId = Course.Id;
            Chapter.CreatedDate = DateTime.UtcNow;

            // Set order (next available number)
            var maxOrder = await _context.Chapters
                .Where(c => c.CourseId == Course.Id)
                .MaxAsync(c => (int?)c.Order) ?? 0;
            Chapter.Order = maxOrder + 1;

            _context.Chapters.Add(Chapter);
            await _context.SaveChangesAsync();

            return RedirectToPage("/TeacherPanel");
        }
    }
}
