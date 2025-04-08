using dbs2webapp.Data;
using dbs2webapp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dbs2webapp.Pages.Courses
{
    // Pages/Courses/Create.cshtml.cs
    [Authorize(Roles = "Teacher,Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Course Course { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Course.TeacherId = _userManager.GetUserId(User);
            Course.CreatedDate = DateTime.UtcNow;

            _context.Courses.Add(Course);
            await _context.SaveChangesAsync();

            return RedirectToPage("/TeacherPanel");
        }
    }
}
