using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dbs2webapp.Models;

namespace dbs2webapp.Controllers
{
    public class TestInstancesController : Controller
    {
        private readonly Dbs2databaseContext _context;

        public TestInstancesController(Dbs2databaseContext context)
        {
            _context = context;
        }

        // GET: TestInstances
        public async Task<IActionResult> Index()
        {
            var dbs2databaseContext = _context.TestInstances.Include(t => t.Test).Include(t => t.User);
            return View(await dbs2databaseContext.ToListAsync());
        }

        // GET: TestInstances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testInstance = await _context.TestInstances
                .Include(t => t.Test)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testInstance == null)
            {
                return NotFound();
            }

            return View(testInstance);
        }

        // GET: TestInstances/Create
        public IActionResult Create()
        {
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: TestInstances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartedAt,Attempt,TestId,UserId")] TestInstance testInstance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testInstance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id", testInstance.TestId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", testInstance.UserId);
            return View(testInstance);
        }

        // GET: TestInstances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testInstance = await _context.TestInstances.FindAsync(id);
            if (testInstance == null)
            {
                return NotFound();
            }
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id", testInstance.TestId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", testInstance.UserId);
            return View(testInstance);
        }

        // POST: TestInstances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartedAt,Attempt,TestId,UserId")] TestInstance testInstance)
        {
            if (id != testInstance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testInstance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestInstanceExists(testInstance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id", testInstance.TestId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", testInstance.UserId);
            return View(testInstance);
        }

        // GET: TestInstances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testInstance = await _context.TestInstances
                .Include(t => t.Test)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testInstance == null)
            {
                return NotFound();
            }

            return View(testInstance);
        }

        // POST: TestInstances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testInstance = await _context.TestInstances.FindAsync(id);
            if (testInstance != null)
            {
                _context.TestInstances.Remove(testInstance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestInstanceExists(int id)
        {
            return _context.TestInstances.Any(e => e.Id == id);
        }
    }
}
