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
    public class ChapterContentsController : Controller
    {
        private readonly Dbs2databaseContext _context;

        public ChapterContentsController(Dbs2databaseContext context)
        {
            _context = context;
        }

        // GET: ChapterContents
        public async Task<IActionResult> Index()
        {
            var dbs2databaseContext = _context.ChapterContents.Include(c => c.Chapter);
            return View(await dbs2databaseContext.ToListAsync());
        }

        // GET: ChapterContents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapterContent = await _context.ChapterContents
                .Include(c => c.Chapter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chapterContent == null)
            {
                return NotFound();
            }

            return View(chapterContent);
        }

        // GET: ChapterContents/Create
        public IActionResult Create()
        {
            ViewData["ChapterId"] = new SelectList(_context.Chapters, "Id", "Id");
            return View();
        }

        // POST: ChapterContents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,TextFile,ChapterId")] ChapterContent chapterContent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chapterContent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChapterId"] = new SelectList(_context.Chapters, "Id", "Id", chapterContent.ChapterId);
            return View(chapterContent);
        }

        // GET: ChapterContents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapterContent = await _context.ChapterContents.FindAsync(id);
            if (chapterContent == null)
            {
                return NotFound();
            }
            ViewData["ChapterId"] = new SelectList(_context.Chapters, "Id", "Id", chapterContent.ChapterId);
            return View(chapterContent);
        }

        // POST: ChapterContents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,TextFile,ChapterId")] ChapterContent chapterContent)
        {
            if (id != chapterContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chapterContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChapterContentExists(chapterContent.Id))
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
            ViewData["ChapterId"] = new SelectList(_context.Chapters, "Id", "Id", chapterContent.ChapterId);
            return View(chapterContent);
        }

        // GET: ChapterContents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapterContent = await _context.ChapterContents
                .Include(c => c.Chapter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chapterContent == null)
            {
                return NotFound();
            }

            return View(chapterContent);
        }

        // POST: ChapterContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chapterContent = await _context.ChapterContents.FindAsync(id);
            if (chapterContent != null)
            {
                _context.ChapterContents.Remove(chapterContent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChapterContentExists(int id)
        {
            return _context.ChapterContents.Any(e => e.Id == id);
        }
    }
}
