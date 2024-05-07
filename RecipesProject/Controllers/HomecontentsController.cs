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
    public class HomecontentsController : Controller
    {
        private readonly ModelContext _context;

        public HomecontentsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Homecontents
        public async Task<IActionResult> Index()
        {
              return _context.Homecontents != null ? 
                          View(await _context.Homecontents.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Homecontents'  is null.");
        }

        // GET: Homecontents/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Homecontents == null)
            {
                return NotFound();
            }

            var homecontent = await _context.Homecontents
                .FirstOrDefaultAsync(m => m.Homecontentid == id);
            if (homecontent == null)
            {
                return NotFound();
            }

            return View(homecontent);
        }

        // GET: Homecontents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Homecontents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Homecontentid,Title,Paragraph")] Homecontent homecontent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homecontent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homecontent);
        }

        // GET: Homecontents/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Homecontents == null)
            {
                return NotFound();
            }

            var homecontent = await _context.Homecontents.FindAsync(id);
            if (homecontent == null)
            {
                return NotFound();
            }
            return View(homecontent);
        }

        // POST: Homecontents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Homecontentid,Title,Paragraph")] Homecontent homecontent)
        {
            if (id != homecontent.Homecontentid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homecontent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomecontentExists(homecontent.Homecontentid))
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
            return View(homecontent);
        }

        // GET: Homecontents/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Homecontents == null)
            {
                return NotFound();
            }

            var homecontent = await _context.Homecontents
                .FirstOrDefaultAsync(m => m.Homecontentid == id);
            if (homecontent == null)
            {
                return NotFound();
            }

            return View(homecontent);
        }

        // POST: Homecontents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Homecontents == null)
            {
                return Problem("Entity set 'ModelContext.Homecontents'  is null.");
            }
            var homecontent = await _context.Homecontents.FindAsync(id);
            if (homecontent != null)
            {
                _context.Homecontents.Remove(homecontent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomecontentExists(decimal id)
        {
          return (_context.Homecontents?.Any(e => e.Homecontentid == id)).GetValueOrDefault();
        }
    }
}
