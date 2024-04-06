using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_3.Data;
using Project_3.Models;

namespace Project_3.Controllers
{
    [Route("Readers")]
    public class ReadersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReadersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Readers
        [HttpGet("Index")]
        public async Task<IActionResult> Index(string searchQuery)
        {
            var readers = _context.Reader.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                readers = readers.Where(r =>
                    r.Name.Contains(searchQuery) ||
                    r.Email.Contains(searchQuery) ||
                    r.Address.Contains(searchQuery) ||
                    r.ContactNumber.Contains(searchQuery));
            }

            return View(await readers.ToListAsync());
        }

        [HttpGet("Details/{id}")]
        // GET: Readers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reader == null)
            {
                return NotFound();
            }

            var reader = await _context.Reader
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reader == null)
            {
                return NotFound();
            }

            return View(reader);
        }

        // GET: Readers/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Readers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Address,ContactNumber")] Reader reader)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reader);
        }

        // GET: Readers/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reader == null)
            {
                return NotFound();
            }

            var reader = await _context.Reader.FindAsync(id);
            if (reader == null)
            {
                return NotFound();
            }
            return View(reader);
        }

        // POST: Readers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Address,ContactNumber")] Reader reader)
        {
            if (id != reader.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReaderExists(reader.Id))
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
            return View(reader);
        }

        // GET: Readers/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reader == null)
            {
                return NotFound();
            }

            var reader = await _context.Reader
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reader == null)
            {
                return NotFound();
            }

            return View(reader);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reader == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reader'  is null.");
            }
            var reader = await _context.Reader.FindAsync(id);
            if (reader != null)
            {
                _context.Reader.Remove(reader);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReaderExists(int id)
        {
            return (_context.Reader?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
