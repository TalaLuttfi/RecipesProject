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
    public class RecipesController : Controller
    {
        private readonly ModelContext _context;

        public RecipesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Recipes.Include(r => r.Category).Include(r => r.Chef);
            return View(await modelContext.ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.Chef)
                .FirstOrDefaultAsync(m => m.Recipeid == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }
        // GET: Recipes/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Recipecategories.ToListAsync();
            ViewBag.Categoryid = new SelectList(categories, "Categoryid", "Categoryname");

            //ViewData["Categoryid"] = new SelectList(_context.Recipecategories, "Categoryid", "Categoryid");
            ViewData["Chefid"] = new SelectList(_context.Users, "Userid", "Userid");

            // Set Approval Status to "Pending" by default
            ViewData["ApprovalStatus"] = "Pending";

            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Recipeid,Chefid,Categoryid,Recipename,Description,Ingredients,Instructions,Imagepath,Price,Approvalstatus")] Recipe recipe)
        {
            int loggedInUserId = HttpContext.Session.GetInt32("Userid") ?? 0; // Default to 0 if session value is null

            if (ModelState.IsValid)
            {
                // Set Approval Status to "Pending" by default
                recipe.Approvalstatus = "Pending";

                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction("SeeRecipe", "Home", new { chefid = loggedInUserId });
            }
            ViewData["Categoryid"] = new SelectList(_context.Recipecategories, "Categoryid", "Categoryid", recipe.Categoryid);
            ViewData["Chefid"] = new SelectList(_context.Users, "Userid", "Userid", recipe.Chefid);
            return RedirectToAction("SeeRecipe", "Home", new { chefid = loggedInUserId });
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["Categoryid"] = new SelectList(_context.Recipecategories, "Categoryid", "Categoryid", recipe.Categoryid);
            ViewData["Chefid"] = new SelectList(_context.Users, "Userid", "Userid", recipe.Chefid);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Recipeid,Chefid,Categoryid,Recipename,Description,Ingredients,Instructions,Imagepath,Price,Approvalstatus")] Recipe recipe)
        {
            int loggedInUserId = HttpContext.Session.GetInt32("Userid") ?? 0; // Default to 0 if session value is null

            if (id != recipe.Recipeid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Recipeid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Recipes", new { chefid = loggedInUserId });
            }
            ViewData["Categoryid"] = new SelectList(_context.Recipecategories, "Categoryid", "Categoryid", recipe.Categoryid);
            ViewData["Chefid"] = new SelectList(_context.Users, "Userid", "Userid", recipe.Chefid);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.Chef)
                .FirstOrDefaultAsync(m => m.Recipeid == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            int loggedInUserId = HttpContext.Session.GetInt32("Userid") ?? 0; // Default to 0 if session value is null

            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound(); // Recipe not found, return appropriate response
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Recipes", new { chefid = loggedInUserId });
        }

        private bool RecipeExists(decimal id)
        {
          return (_context.Recipes?.Any(e => e.Recipeid == id)).GetValueOrDefault();
        }
    }
}
