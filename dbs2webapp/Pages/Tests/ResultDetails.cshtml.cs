using dbs2webapp.Data;
using dbs2webapp.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dbs2webapp.Pages.Tests;

public class ResultDetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ResultDetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public TestResult TestResult { get; set; }
    public Test Test { get; set; }

    public async Task<IActionResult> OnGetAsync(int resultId)
    {
        TestResult = await _context.TestResults
            .Include(tr => tr.Test)
                .ThenInclude(t => t.Questions)
                    .ThenInclude(q => q.Options)
            .Include(tr => tr.Test.Chapter)
            .FirstOrDefaultAsync(tr => tr.Id == resultId);

        if (TestResult == null)
        {
            return NotFound();
        }

        Test = TestResult.Test;
        return Page();
    }
}