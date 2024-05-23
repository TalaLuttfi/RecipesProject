using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesProject.Models;
using System.Diagnostics;
using RecipesProject.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipesProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
    


        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
   
        _logger = logger;
            _context = context;
        }


		public async Task<IActionResult> Index()
		{
			var combinedTestimonialsData = await _context.Testimonials
				.Include(t => t.User)
				.Where(t => t.Approvalstatus == "Approved")
				.Select(t => new Tuple<Testimonial, User>(t, t.User))
				.ToListAsync();

			var chefs = await _context.Users
				.Where(u => u.Roleid == 2)
				.ToListAsync();

			var recipes = await _context.Recipes
				.ToListAsync();

			// Combine testimonials data with users and recipes
			var combinedData = new Tuple<List<Tuple<Testimonial, User>>, List<User>, List<Recipe>>(
				combinedTestimonialsData, chefs, recipes);

			return View(combinedData);
		}

		public async Task<IActionResult> User()
		{

			var combinedTestimonialsData = await _context.Testimonials
				.Include(t => t.User)
				.Where(t => t.Approvalstatus == "Approved")
				.Select(t => new Tuple<Testimonial, User>(t, t.User))
				.ToListAsync();

			var chefs = await _context.Users
				.Where(u => u.Roleid == 2)
				.ToListAsync();

			var recipes = await _context.Recipes
				.ToListAsync();

			// Combine testimonials data with users and recipes
			var combinedData = new Tuple<List<Tuple<Testimonial, User>>, List<User>, List<Recipe>>(
				combinedTestimonialsData, chefs, recipes);

			return View(combinedData);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		
		public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> BuyRecipe(Recipe recipe)
        {

            var RecipePrice = await _context.Recipes
        .Where(r => r.Recipeid == recipe.Recipeid)
        .Select(r => r.Price)
        .FirstOrDefaultAsync();
            ViewBag.RecipePrice = RecipePrice;

            return View();
        }


        public async Task<IActionResult> SeeRecipe(decimal chefid, string category)
        {
            // Retrieve categories from the database asynchronously
            var categories = await _context.Recipecategories.Select(c => c.Categoryname).ToListAsync();
            ViewBag.Categories = categories;

            // Retrieve recipes for the selected chef with an approval status of "Approved" from the database asynchronously
            var recipesQuery = _context.Recipes
                                       .Where(r => r.Chefid == chefid && r.Approvalstatus == "Approved"); // Filter by chefId and approval status

            // Retrieve recipes from the database asynchronously
            var recipes = await recipesQuery.ToListAsync();

            // Pass the list of approved recipes to the view along with the logged-in chef's ID
            ViewBag.ChefId = chefid;
            return View(recipes);
        }


        public async Task<IActionResult> Myrecipe()
        {

            // Retrieve chefs with role ID 3 from the database asynchronously
            var chefs = await _context.Users.Where(u => u.Roleid == 2).ToListAsync();

            // Pass the list of chefs to the view
            return View(chefs);
        }

		public async Task<IActionResult> Chefindex()
        {


			var combinedTestimonialsData = await _context.Testimonials
				.Include(t => t.User)
				.Where(t => t.Approvalstatus == "Approved")
				.Select(t => new Tuple<Testimonial, User>(t, t.User))
				.ToListAsync();

			var chefs = await _context.Users
				.Where(u => u.Roleid == 2)
				.ToListAsync();

			var recipes = await _context.Recipes
				.ToListAsync();

			// Combine testimonials data with users and recipes
			var combinedData = new Tuple<List<Tuple<Testimonial, User>>, List<User>, List<Recipe>>(
				combinedTestimonialsData, chefs, recipes);

			return View(combinedData);
		}
        public async Task<IActionResult> Chef()
        {
            // Retrieve chefs with role ID 3 from the database asynchronously
            var chefs = await _context.Users.Where(u => u.Roleid == 2).ToListAsync();

            // Pass the list of chefs to the view
            return View(chefs);
        }

        public async Task<IActionResult> ChefRecipes(decimal chefid, string category)
        {    // Retrieve categories from the database asynchronously
            var categories = await _context.Recipecategories.Select(c => c.Categoryname).ToListAsync();
            ViewBag.Categories = categories;

            // Retrieve recipes for the selected chef with an approval status of "Approved" from the database asynchronously
            var recipesQuery = _context.Recipes
                                       .Where(r => r.Chefid == chefid && r.Approvalstatus == "Approved"); // Filter by chefId and approval status

            // Retrieve recipes from the database asynchronously
            var recipes = await recipesQuery.ToListAsync();

            // Pass the list of approved recipes to the view along with the logged-in chef's ID
            ViewBag.ChefId = chefid;
            return View(recipes);
        }
        public async Task<IActionResult> Recipes()
        {
            var categories = await _context.Recipecategories
                .Select(c => c.Categoryname)
                .Distinct()
                .ToListAsync();

            ViewBag.Categories = categories; // Use the list directly

            var approvedRecipes = await _context.Recipes
                .Where(r => r.Approvalstatus == "Approved")
                .ToListAsync();

            return View(approvedRecipes);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword, string category)
        {
            var recipes = _context.Recipes
                .Include(r => r.Category) // Ensure Category is included to filter by category name
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                var lowerKeyword = keyword.ToLower();
                recipes = recipes.Where(r => r.Recipename.ToLower().Contains(lowerKeyword) ||
                                             r.Description.ToLower().Contains(lowerKeyword) ||
                                             r.Ingredients.ToLower().Contains(lowerKeyword) ||
                                             r.Instructions.ToLower().Contains(lowerKeyword));
            }

            if (!string.IsNullOrEmpty(category))
            {
                var lowerCategory = category.ToLower();
                recipes = recipes.Where(r => r.Category.Categoryname.ToLower() == lowerCategory);
            }

            var filteredRecipes = await recipes.ToListAsync();

            var categories = await _context.Recipecategories
                .Select(c => c.Categoryname)
                .Distinct()
                .ToListAsync();

            ViewBag.Categories = categories; // Use the list directly

            return View("Recipes", filteredRecipes);
        }


        //// for admin approve recipe 
        [HttpGet]
        public async Task<IActionResult> ReviewRecipes()
        {
            var pendingRecipes = await _context.Recipes.Where(r => r.Approvalstatus == "Pending").ToListAsync();
            return View(pendingRecipes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            recipe.Approvalstatus = "Approved";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ReviewRecipes));
        }


        public IActionResult Contact()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            var About = _context.Aboutcontents.ToList();

            // Get counts from the database using LINQ queries
            int totalUsers = await _context.Users.CountAsync();
            int totalRecipes = await _context.Recipes.CountAsync();
            int totalsold = await _context.Recipes.CountAsync();

            // Pass the counts to the view
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalRecipes = totalRecipes;
            ViewBag.TotalSold = totalsold;


            var users = await _context.Users.Include(u => u.Role).ToListAsync();

            return View(About);
        }
        public IActionResult sendemail()
        {

            return View();
        }
		public async Task<IActionResult> MoreInfo(int id)
		{
			var recipe = await _context.Recipes
				.FirstOrDefaultAsync(r => r.Recipeid == id && r.Price == 0);

			if (recipe == null)
			{
				return NotFound();
			}

			return View(recipe);
		}

        public async Task<IActionResult> Recipestwo()
        {
            var categories = await _context.Recipecategories
                .Select(c => c.Categoryname)
                .Distinct()
                .ToListAsync();

            ViewBag.Categories = categories; // Use the list directly

            var approvedRecipes = await _context.Recipes
                .Where(r => r.Approvalstatus == "Approved")
                .ToListAsync();

            return View(approvedRecipes);
        }


    }
} 