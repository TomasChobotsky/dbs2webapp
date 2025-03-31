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
    public class AssignmentUsersController : Controller
    {
        private readonly Dbs2databaseContext _context;

        public AssignmentUsersController(Dbs2databaseContext context)
        {
            _context = context;
        }

        // GET: AssignmentUsers
        public async Task<IActionResult> Index()
        {
            var dbs2databaseContext = _context.AssignmentUsers.Include(a => a.Assignment).Include(a => a.User);
            return View(await dbs2databaseContext.ToListAsync());
        }

        // GET: AssignmentUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentUser = await _context.AssignmentUsers
                .Include(a => a.Assignment)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignmentUser == null)
            {
                return NotFound();
            }

            return View(assignmentUser);
        }

        // GET: AssignmentUsers/Create
        public IActionResult Create()
        {
            ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: AssignmentUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AssignmentId,UserId")] AssignmentUser assignmentUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignmentUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Id", assignmentUser.AssignmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", assignmentUser.UserId);
            return View(assignmentUser);
        }

        // GET: AssignmentUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentUser = await _context.AssignmentUsers.FindAsync(id);
            if (assignmentUser == null)
            {
                return NotFound();
            }
            ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Id", assignmentUser.AssignmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", assignmentUser.UserId);
            return View(assignmentUser);
        }

        // POST: AssignmentUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AssignmentId,UserId")] AssignmentUser assignmentUser)
        {
            if (id != assignmentUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignmentUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentUserExists(assignmentUser.Id))
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
            ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Id", assignmentUser.AssignmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", assignmentUser.UserId);
            return View(assignmentUser);
        }

        // GET: AssignmentUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentUser = await _context.AssignmentUsers
                .Include(a => a.Assignment)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignmentUser == null)
            {
                return NotFound();
            }

            return View(assignmentUser);
        }

        // POST: AssignmentUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignmentUser = await _context.AssignmentUsers.FindAsync(id);
            if (assignmentUser != null)
            {
                _context.AssignmentUsers.Remove(assignmentUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentUserExists(int id)
        {
            return _context.AssignmentUsers.Any(e => e.Id == id);
        }
    }
}
