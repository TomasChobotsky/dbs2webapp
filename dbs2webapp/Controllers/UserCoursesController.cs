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
    public class UserCoursesController : Controller
    {
        private readonly Dbs2databaseContext _context;

        public UserCoursesController(Dbs2databaseContext context)
        {
            _context = context;
        }

        // GET: UserCourses
        public async Task<IActionResult> Index()
        {
            var dbs2databaseContext = _context.UserCourses.Include(u => u.Course).Include(u => u.User);
            return View(await dbs2databaseContext.ToListAsync());
        }

        // GET: UserCourses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCourse = await _context.UserCourses
                .Include(u => u.Course)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userCourse == null)
            {
                return NotFound();
            }

            return View(userCourse);
        }

        // GET: UserCourses/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,CourseId")] UserCourse userCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", userCourse.CourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCourse.UserId);
            return View(userCourse);
        }

        // GET: UserCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCourse = await _context.UserCourses.FindAsync(id);
            if (userCourse == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", userCourse.CourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCourse.UserId);
            return View(userCourse);
        }

        // POST: UserCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,CourseId")] UserCourse userCourse)
        {
            if (id != userCourse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCourseExists(userCourse.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", userCourse.CourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCourse.UserId);
            return View(userCourse);
        }

        // GET: UserCourses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCourse = await _context.UserCourses
                .Include(u => u.Course)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userCourse == null)
            {
                return NotFound();
            }

            return View(userCourse);
        }

        // POST: UserCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userCourse = await _context.UserCourses.FindAsync(id);
            if (userCourse != null)
            {
                _context.UserCourses.Remove(userCourse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserCourseExists(int id)
        {
            return _context.UserCourses.Any(e => e.Id == id);
        }
    }
}
