using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_3.Data;
using Project_3.Models;

namespace Project_3.Controllers
{
    [Route("Borrowings")]
    public class BorrowingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BorrowingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Borrowings
        [HttpGet("Index")]
        public async Task<IActionResult> Index(string searchQuery)
        {
            var borrowings = _context.Borrowing
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                borrowings = borrowings.Where(b =>
                    b.Book.Title.Contains(searchQuery) ||
                    b.Reader.Name.Contains(searchQuery) ||
                    b.BorrowDate.ToString().Contains(searchQuery) ||
                    b.DueDate.ToString().Contains(searchQuery) ||
                    (b.ReturnDate.HasValue && b.ReturnDate.Value.ToString().Contains(searchQuery)));
            }

            return View(await borrowings.ToListAsync());
        }

        // GET: Borrowings/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Borrowing == null)
            {
                return NotFound();
            }

            var borrowing = await _context.Borrowing
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowing == null)
            {
                return NotFound();
            }

            return View(borrowing);
        }

        // GET: Borrowings/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Author");
            ViewData["ReaderId"] = new SelectList(_context.Set<Reader>(), "Id", "Address");
            return View();
        }

        // POST: Borrowings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,ReaderId,BorrowDate,DueDate,ReturnDate")] Borrowing borrowing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(borrowing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Author", borrowing.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Set<Reader>(), "Id", "Address", borrowing.ReaderId);
            return View(borrowing);
        }

        // GET: Borrowings/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Borrowing == null)
            {
                return NotFound();
            }

            var borrowing = await _context.Borrowing.FindAsync(id);
            if (borrowing == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Author", borrowing.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Set<Reader>(), "Id", "Address", borrowing.ReaderId);
            return View(borrowing);
        }

        // POST: Borrowings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,ReaderId,BorrowDate,DueDate,ReturnDate")] Borrowing borrowing)
        {
            if (id != borrowing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrowing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowingExists(borrowing.Id))
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
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Author", borrowing.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Set<Reader>(), "Id", "Address", borrowing.ReaderId);
            return View(borrowing);
        }

        // GET: Borrowings/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Borrowing == null)
            {
                return NotFound();
            }

            var borrowing = await _context.Borrowing
                .Include(b => b.Book)
                .Include(b => b.Reader)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowing == null)
            {
                return NotFound();
            }

            return View(borrowing);
        }

        // POST: Borrowings/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Borrowing == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Borrowing'  is null.");
            }
            var borrowing = await _context.Borrowing.FindAsync(id);
            if (borrowing != null)
            {
                _context.Borrowing.Remove(borrowing);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowingExists(int id)
        {
            return (_context.Borrowing?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
