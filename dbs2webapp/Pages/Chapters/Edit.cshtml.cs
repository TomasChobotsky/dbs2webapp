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
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EditModel(ApplicationDbContext context,
                       UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Chapter Chapter { get; set; }

        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Chapter = await _context.Chapters
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Chapter == null)
            {
                return NotFound();
            }

            // Verify current user owns the course or is admin
            var currentUserId = _userManager.GetUserId(User);
            if (Chapter.Course.TeacherId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            Course = Chapter.Course;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(List<IFormFile> imageFiles)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Get existing chapter to preserve CourseId
            var existingChapter = await _context.Chapters
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == Chapter.Id);

            if (existingChapter == null)
            {
                return NotFound();
            }

            // Only update editable fields
            Chapter.CourseId = existingChapter.CourseId;
            Chapter.CreatedDate = existingChapter.CreatedDate;
            Chapter.Order = existingChapter.Order;
            Chapter.Content = existingChapter.Content;

            _context.Attach(Chapter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChapterExists(Chapter.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToPage("/TeacherPanel");
        }

        private bool ChapterExists(int id)
        {
            return _context.Chapters.Any(e => e.Id == id);
        }
    }
}
