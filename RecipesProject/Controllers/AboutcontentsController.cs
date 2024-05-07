using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipesProject.Models;

namespace RecipesProject.Controllers
{
    public class AboutcontentsController : Controller
    {
        private readonly ModelContext _context;

        public AboutcontentsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Aboutcontents
        public async Task<IActionResult> Index()
        {
              return _context.Aboutcontents != null ? 
                          View(await _context.Aboutcontents.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Aboutcontents'  is null.");
        }

        // GET: Aboutcontents/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Aboutcontents == null)
            {
                return NotFound();
            }

            var aboutcontent = await _context.Aboutcontents
                .FirstOrDefaultAsync(m => m.Aboutcontentid == id);
            if (aboutcontent == null)
            {
                return NotFound();
            }

            return View(aboutcontent);
        }

        // GET: Aboutcontents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aboutcontents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Aboutcontentid,Title,Paragraph")] Aboutcontent aboutcontent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aboutcontent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutcontent);
        }

        // GET: Aboutcontents/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Aboutcontents == null)
            {
                return NotFound();
            }

            var aboutcontent = await _context.Aboutcontents.FindAsync(id);
            if (aboutcontent == null)
            {
                return NotFound();
            }
            return View(aboutcontent);
        }

        // POST: Aboutcontents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Aboutcontentid,Title,Paragraph")] Aboutcontent aboutcontent)
        {
            if (id != aboutcontent.Aboutcontentid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aboutcontent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutcontentExists(aboutcontent.Aboutcontentid))
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
            return View(aboutcontent);
        }

        // GET: Aboutcontents/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Aboutcontents == null)
            {
                return NotFound();
            }

            var aboutcontent = await _context.Aboutcontents
                .FirstOrDefaultAsync(m => m.Aboutcontentid == id);
            if (aboutcontent == null)
            {
                return NotFound();
            }

            return View(aboutcontent);
        }

        // POST: Aboutcontents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Aboutcontents == null)
            {
                return Problem("Entity set 'ModelContext.Aboutcontents'  is null.");
            }
            var aboutcontent = await _context.Aboutcontents.FindAsync(id);
            if (aboutcontent != null)
            {
                _context.Aboutcontents.Remove(aboutcontent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutcontentExists(decimal id)
        {
          return (_context.Aboutcontents?.Any(e => e.Aboutcontentid == id)).GetValueOrDefault();
        }
    }
}
