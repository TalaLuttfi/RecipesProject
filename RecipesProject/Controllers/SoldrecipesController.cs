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
    public class SoldrecipesController : Controller
    {
        private readonly ModelContext _context;

        public SoldrecipesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Soldrecipes
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Soldrecipes.Include(s => s.Buyer).Include(s => s.Recipe);
            return View(await modelContext.ToListAsync());
        }

        // GET: Soldrecipes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Soldrecipes == null)
            {
                return NotFound();
            }

            var soldrecipe = await _context.Soldrecipes
                .Include(s => s.Buyer)
                .Include(s => s.Recipe)
                .FirstOrDefaultAsync(m => m.Soldrecipeid == id);
            if (soldrecipe == null)
            {
                return NotFound();
            }

            return View(soldrecipe);
        }

        // GET: Soldrecipes/Create
        public IActionResult Create()
        {
            ViewData["Buyerid"] = new SelectList(_context.Users, "Userid", "Userid");
            ViewData["Recipeid"] = new SelectList(_context.Recipes, "Recipeid", "Recipeid");
            return View();
        }

        // POST: Soldrecipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyRecipe([Bind("Recipeid,Buyerid,Purchasedate")] Soldrecipe soldrecipe, Payment payment, Visacard visa)
        {
            if (ModelState.IsValid)
            {
                decimal recipeId = Convert.ToDecimal(HttpContext.Request.Form["RecipeId"]);
                decimal recipePrice = Convert.ToDecimal(HttpContext.Request.Form["RecipePrice"]);

                var existingVisa = await _context.Visacards.FirstOrDefaultAsync(v => v.Cardnumber == visa.Cardnumber);
                if (existingVisa != null && existingVisa.Balance >= recipePrice)
                {
                    // Update VisaCard balance
                    existingVisa.Balance -= recipePrice;

                    // Set Soldrecipe details
                    soldrecipe.Recipeid = recipeId;
                    soldrecipe.Buyerid = HttpContext.Session.GetInt32("Userid");
                    soldrecipe.Purchasedate = DateTime.Now;

                    // Set Payment details
                    payment.Recipeid = recipeId;
                    payment.Userid = HttpContext.Session.GetInt32("Userid");
                    payment.Cardid = existingVisa.Cardid;
                    payment.Amount = recipePrice;
                    payment.Paymentdate = DateTime.Now;

                    _context.Add(soldrecipe);
                    _context.Add(payment);

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Payment successful!";
                    return RedirectToAction("Index", "Home"); // Redirect to a suitable page after payment
                }
                else
                {
                    TempData["ErrorMessage"] = "Insufficient balance or invalid card number.";
                }
            }

            // If ModelState is not valid, return the view with validation errors
            // Add necessary ViewData here if needed
            return View();
        }



        // GET: Soldrecipes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Soldrecipes == null)
            {
                return NotFound();
            }

            var soldrecipe = await _context.Soldrecipes.FindAsync(id);
            if (soldrecipe == null)
            {
                return NotFound();
            }
            ViewData["Buyerid"] = new SelectList(_context.Users, "Userid", "Userid", soldrecipe.Buyerid);
            ViewData["Recipeid"] = new SelectList(_context.Recipes, "Recipeid", "Recipeid", soldrecipe.Recipeid);
            return View(soldrecipe);
        }

        // POST: Soldrecipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Soldrecipeid,Recipeid,Buyerid,Purchasedate")] Soldrecipe soldrecipe)
        {
            if (id != soldrecipe.Soldrecipeid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(soldrecipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoldrecipeExists(soldrecipe.Soldrecipeid))
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
            ViewData["Buyerid"] = new SelectList(_context.Users, "Userid", "Userid", soldrecipe.Buyerid);
            ViewData["Recipeid"] = new SelectList(_context.Recipes, "Recipeid", "Recipeid", soldrecipe.Recipeid);
            return View(soldrecipe);
        }

        // GET: Soldrecipes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Soldrecipes == null)
            {
                return NotFound();
            }

            var soldrecipe = await _context.Soldrecipes
                .Include(s => s.Buyer)
                .Include(s => s.Recipe)
                .FirstOrDefaultAsync(m => m.Soldrecipeid == id);
            if (soldrecipe == null)
            {
                return NotFound();
            }

            return View(soldrecipe);
        }

        // POST: Soldrecipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Soldrecipes == null)
            {
                return Problem("Entity set 'ModelContext.Soldrecipes'  is null.");
            }
            var soldrecipe = await _context.Soldrecipes.FindAsync(id);
            if (soldrecipe != null)
            {
                _context.Soldrecipes.Remove(soldrecipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoldrecipeExists(decimal id)
        {
          return (_context.Soldrecipes?.Any(e => e.Soldrecipeid == id)).GetValueOrDefault();
        }
    }
}
