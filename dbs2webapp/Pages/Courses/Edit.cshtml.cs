using dbs2webapp.Data;
using dbs2webapp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace dbs2webapp.Pages.Courses
{
    // Pages/Courses/Edit.cshtml.cs
    [Authorize(Roles = "Teacher,Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Course = await _context.Courses.FirstOrDefaultAsync(m => m.Id == id);

            if (Course == null)
                return NotFound();

            // Only allow the course creator or admin to edit
            var currentUserId = _userManager.GetUserId(User);
            if (Course.TeacherId != currentUserId && !User.IsInRole("Admin"))
                return Forbid();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var courseToUpdate = await _context.Courses.FindAsync(Course.Id);

            if (courseToUpdate == null)
                return NotFound();

            // Only update allowed fields
            courseToUpdate.Name = Course.Name;
            courseToUpdate.Description = Course.Description;
            // Don't update TeacherId or CreatedDate

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(Course.Id))
                    return NotFound();
                throw;
            }

            return RedirectToPage("/TeacherPanel");
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
