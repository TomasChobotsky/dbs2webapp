using dbs2webapp.Data;
using dbs2webapp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dbs2webapp.Pages.Tests
{
    public class HistoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HistoryModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<TestResult> TestResults { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                RedirectToPage("/Account/Login", new { area = "Identity" });
                return;
            }

            TestResults = await _context.TestResults
                .Include(tr => tr.Test)
                    .ThenInclude(t => t.Chapter)
                .Where(tr => tr.UserId == user.Id)
                .OrderByDescending(tr => tr.CompletedDate)
                .ToListAsync();
        }
    }
}