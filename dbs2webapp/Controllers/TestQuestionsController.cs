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
    public class TestQuestionsController : Controller
    {
        private readonly Dbs2databaseContext _context;

        public TestQuestionsController(Dbs2databaseContext context)
        {
            _context = context;
        }

        // GET: TestQuestions
        public async Task<IActionResult> Index()
        {
            var dbs2databaseContext = _context.TestQuestions.Include(t => t.Question).Include(t => t.Test);
            return View(await dbs2databaseContext.ToListAsync());
        }

        // GET: TestQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testQuestion = await _context.TestQuestions
                .Include(t => t.Question)
                .Include(t => t.Test)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testQuestion == null)
            {
                return NotFound();
            }

            return View(testQuestion);
        }

        // GET: TestQuestions/Create
        public IActionResult Create()
        {
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id");
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id");
            return View();
        }

        // POST: TestQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QuestionId,TestId")] TestQuestion testQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", testQuestion.QuestionId);
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id", testQuestion.TestId);
            return View(testQuestion);
        }

        // GET: TestQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testQuestion = await _context.TestQuestions.FindAsync(id);
            if (testQuestion == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", testQuestion.QuestionId);
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id", testQuestion.TestId);
            return View(testQuestion);
        }

        // POST: TestQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuestionId,TestId")] TestQuestion testQuestion)
        {
            if (id != testQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestQuestionExists(testQuestion.Id))
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
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", testQuestion.QuestionId);
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id", testQuestion.TestId);
            return View(testQuestion);
        }

        // GET: TestQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testQuestion = await _context.TestQuestions
                .Include(t => t.Question)
                .Include(t => t.Test)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testQuestion == null)
            {
                return NotFound();
            }

            return View(testQuestion);
        }

        // POST: TestQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testQuestion = await _context.TestQuestions.FindAsync(id);
            if (testQuestion != null)
            {
                _context.TestQuestions.Remove(testQuestion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestQuestionExists(int id)
        {
            return _context.TestQuestions.Any(e => e.Id == id);
        }
    }
}
