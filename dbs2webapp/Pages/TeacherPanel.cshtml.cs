using dbs2webapp.Data;
using dbs2webapp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dbs2webapp.Pages
{
    [Authorize(Roles = "Teacher,Admin")]
    // This page is for teachers to manage their courses and chapters.
    // It displays a list of courses created by the teacher.
    // The teacher can view the chapters and tests associated with each course.
    public class TeacherPanelModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TeacherPanelModel(ApplicationDbContext context,
                               UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Course> Courses { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            Courses = await _context.Courses
                .Include(c => c.Chapters)
                    .ThenInclude(ch => ch.Tests)
                .Where(c => c.TeacherId == userId)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }
    }
}