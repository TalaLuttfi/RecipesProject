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
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public TestimonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Testimonials
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Testimonials.Include(t => t.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Testimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Testimonialid == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

		// GET: Testimonials/Create
		// POST: Testimonials/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Testimonialid,Testimonialtext")] Testimonial testimonial)
		{
			if (ModelState.IsValid)
			{
				// Retrieve Userid from session
				int? userid = HttpContext.Session.GetInt32("Userid");

				// Check if UserId is available in session
				if (userid.HasValue)
				{
					testimonial.Userid = userid.Value; // Assign Userid to testimonial
					testimonial.Approvalstatus = "0"; // Set approval status to zero
					_context.Add(testimonial);
					await _context.SaveChangesAsync();
					return RedirectToAction("User", "Home");
				}
				else
				{
					// Handle scenario where Userid is not available in session
					TempData["ErrorMessage"] = "User session expired. Please log in again.";
					return RedirectToAction("Login", "LoginAndRegister");
				}
			}
			return View(testimonial);
		}


		// GET: Testimonials/Edit/5
		public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", testimonial.Userid);
            return View(testimonial);
        }

        // POST: Testimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Testimonialid,Userid,Testimonialtext,Approvalstatus")] Testimonial testimonial)
        {
            if (id != testimonial.Testimonialid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Testimonialid))
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
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", testimonial.Userid);
            return View(testimonial);
        }

        // GET: Testimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Testimonialid == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Testimonials == null)
            {
                return Problem("Entity set 'ModelContext.Testimonials'  is null.");
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(decimal id)
        {
          return (_context.Testimonials?.Any(e => e.Testimonialid == id)).GetValueOrDefault();
        }
    }
}
