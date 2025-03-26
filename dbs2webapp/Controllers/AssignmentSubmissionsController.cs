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
    public class AssignmentSubmissionsController : Controller
    {
        private readonly Dbs2databaseContext _context;

        public AssignmentSubmissionsController(Dbs2databaseContext context)
        {
            _context = context;
        }

        // GET: AssignmentSubmissions
        public async Task<IActionResult> Index()
        {
            var dbs2databaseContext = _context.AssignmentSubmissions.Include(a => a.Assignment).Include(a => a.User);
            return View(await dbs2databaseContext.ToListAsync());
        }

        // GET: AssignmentSubmissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentSubmission = await _context.AssignmentSubmissions
                .Include(a => a.Assignment)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignmentSubmission == null)
            {
                return NotFound();
            }

            return View(assignmentSubmission);
        }

        // GET: AssignmentSubmissions/Create
        public IActionResult Create()
        {
            ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: AssignmentSubmissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,File,AssignmentId,UserId")] AssignmentSubmission assignmentSubmission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignmentSubmission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Id", assignmentSubmission.AssignmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", assignmentSubmission.UserId);
            return View(assignmentSubmission);
        }

        // GET: AssignmentSubmissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentSubmission = await _context.AssignmentSubmissions.FindAsync(id);
            if (assignmentSubmission == null)
            {
                return NotFound();
            }
            ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Id", assignmentSubmission.AssignmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", assignmentSubmission.UserId);
            return View(assignmentSubmission);
        }

        // POST: AssignmentSubmissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,File,AssignmentId,UserId")] AssignmentSubmission assignmentSubmission)
        {
            if (id != assignmentSubmission.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignmentSubmission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentSubmissionExists(assignmentSubmission.Id))
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
            ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Id", assignmentSubmission.AssignmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", assignmentSubmission.UserId);
            return View(assignmentSubmission);
        }

        // GET: AssignmentSubmissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentSubmission = await _context.AssignmentSubmissions
                .Include(a => a.Assignment)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignmentSubmission == null)
            {
                return NotFound();
            }

            return View(assignmentSubmission);
        }

        // POST: AssignmentSubmissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignmentSubmission = await _context.AssignmentSubmissions.FindAsync(id);
            if (assignmentSubmission != null)
            {
                _context.AssignmentSubmissions.Remove(assignmentSubmission);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentSubmissionExists(int id)
        {
            return _context.AssignmentSubmissions.Any(e => e.Id == id);
        }
    }
}
