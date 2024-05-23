using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using RecipesProject.Models;

namespace RecipesProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;

        public AdminController(ModelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get counts from the database using LINQ queries
            int totalUsers = await _context.Users.CountAsync();
            int totalChefs = await _context.Users.Where(u => u.Role != null && u.Role.Rolename == "Chef").CountAsync();
            int totalRecipes = await _context.Recipes.CountAsync();
            int totalsoldrecipes = await _context.Soldrecipes.CountAsync();

            // Pass the counts to the view
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalChefs = totalChefs;
            ViewBag.TotalRecipes = totalRecipes;
            ViewBag.totalsoldrecipes = totalsoldrecipes;
            var users = await _context.Users.Include(u => u.Role).ToListAsync();
            return View(users);
        }

        // GET: Users/Edit/5
        // GET: Users/Edit/5
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(decimal id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "Roleid", user.Roleid);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Userid,Username,Email,Password,Roleid,Imagepath,Description")] User user)
        {
            // Check if the retrieved user is null
            var originalUser = await _context.Users.FindAsync(id);
            if (originalUser == null)
            {
                return NotFound();
            }

            // Ensure that only one entity instance with a given key value is attached
            _context.Entry(originalUser).State = EntityState.Detached;

            // Set the role ID of the user to the original role ID
            user.Roleid = originalUser.Roleid;

            // Set the user ID to the retrieved ID
            user.Userid = id;

            // Check if the retrieved user ID is null or not
            if (user.Userid == null)
            {
                // Handle the case where user ID is not found in the session
                return RedirectToAction("Login", "Account"); // Redirect to login page or handle accordingly
            }

            if (id != user.Userid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Userid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Admin");
            }

            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "Roleid", user.Roleid);

            // Pass the user ID to the view
            ViewData["Userid"] = user.Userid;

            return View(user);
        }

        private bool UserExists(decimal id)
        {
            return (_context.Users?.Any(e => e.Userid == id)).GetValueOrDefault();
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        // GET: Admin/PendingRecipes
        public async Task<IActionResult> PendingRecipes()
        {
            // Retrieve all pending recipes
            var pendingRecipes = await _context.Recipes.Where(r => r.Approvalstatus == "Pending").ToListAsync();
            return View(pendingRecipes);
        }

        // POST: Admin/ApproveRecipe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveRecipe(decimal recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe != null)
            {
                // Update approval status to "Approved"
                recipe.Approvalstatus = "Approved";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(PendingRecipes));
        }

        // POST: Admin/DeleteRecipe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecipe(decimal recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(PendingRecipes));
        }
   
   


    }
}
