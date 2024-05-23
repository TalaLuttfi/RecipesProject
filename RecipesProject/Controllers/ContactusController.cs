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
    public class ContactusController : Controller
    {
        private readonly ModelContext _context;

        public ContactusController(ModelContext context)
        {
            _context = context;
        }

        // GET: Contactus
        public async Task<IActionResult> Index()
        {
              return _context.Contactus != null ? 
                          View(await _context.Contactus.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Contactus'  is null.");
        }

        // GET: Contactus/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Contactus == null)
            {
                return NotFound();
            }

            var contactu = await _context.Contactus
                .FirstOrDefaultAsync(m => m.Contactusid == id);
            if (contactu == null)
            {
                return NotFound();
            }

            return View(contactu);
        }

        // GET: Contactus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contactus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Contactusid,Message,Phonenumber,Senderemail")] Contactu contactu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactu);
                await _context.SaveChangesAsync();

                // Add a success message to TempData
                TempData["SuccessMessage"] = "Your message was sent successfully.";

                // Redirect back to the same page
                return RedirectToAction("Contact", "Home");
            }
            return View(contactu);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateIn([Bind("Contactusid,Message,Phonenumber,Senderemail")] Contactu contactu)
		{
			if (ModelState.IsValid)
			{
				_context.Add(contactu);
				await _context.SaveChangesAsync();

				// Add a success message to TempData
				TempData["SuccessMessage"] = "Your message was sent successfully.";

				// Redirect back to the same page
				return RedirectToAction("Index", "Home");
			}
			return View(contactu);
		}

		// GET: Contactus/Edit/5
		public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Contactus == null)
            {
                return NotFound();
            }

            var contactu = await _context.Contactus.FindAsync(id);
            if (contactu == null)
            {
                return NotFound();
            }
            return View(contactu);
        }

        // POST: Contactus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Contactusid,Message,Phonenumber,Senderemail")] Contactu contactu)
        {
            if (id != contactu.Contactusid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactuExists(contactu.Contactusid))
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
            return View(contactu);
        }

        // GET: Contactus/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Contactus == null)
            {
                return NotFound();
            }

            var contactu = await _context.Contactus
                .FirstOrDefaultAsync(m => m.Contactusid == id);
            if (contactu == null)
            {
                return NotFound();
            }

            return View(contactu);
        }

        // POST: Contactus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Contactus == null)
            {
                return Problem("Entity set 'ModelContext.Contactus'  is null.");
            }
            var contactu = await _context.Contactus.FindAsync(id);
            if (contactu != null)
            {
                _context.Contactus.Remove(contactu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactuExists(decimal id)
        {
          return (_context.Contactus?.Any(e => e.Contactusid == id)).GetValueOrDefault();
        }
    }
}
