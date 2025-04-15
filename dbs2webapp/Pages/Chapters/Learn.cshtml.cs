using dbs2webapp.Data;
using dbs2webapp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dbs2webapp.Pages.Chapters
{
    [Authorize(Roles = "Student")]
    public class LearnModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LearnModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Chapter Chapter { get; set; }

        // New property to hold the list of tests for this chapter.
        public List<Test> Tests { get; set; } = new List<Test>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Load the chapter along with its Course for display
            Chapter = await _context.Chapters
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Chapter == null)
            {
                return NotFound();
            }

            // Fetch all tests for this chapter.
            Tests = await _context.Tests
                .Where(t => t.ChapterId == id)
                .ToListAsync();

            return Page();
        }
    }
}
