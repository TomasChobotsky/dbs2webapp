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

        public CreateModel(ApplicationDbContext context,
                         UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Chapter Chapter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CourseId { get; set; }

        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Verify course exists and user has permission
            Course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == CourseId);

            if (Course == null)
            {
                return NotFound();
            }

            // Check if current user is the course teacher or admin
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
                return Page();
            }

            // Set course ID and creation date
            Chapter.CourseId = CourseId;
            Chapter.CreatedDate = DateTime.UtcNow;

            // Set order (next available number)
            var maxOrder = await _context.Chapters
                .Where(c => c.CourseId == CourseId)
                .MaxAsync(c => (int?)c.Order) ?? 0;
            Chapter.Order = maxOrder + 1;

            _context.Chapters.Add(Chapter);
            await _context.SaveChangesAsync();

            return RedirectToPage("/TeacherPanel");
        }
    }
}
