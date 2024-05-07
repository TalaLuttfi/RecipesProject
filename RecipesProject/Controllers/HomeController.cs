using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesProject.Models;
using System.Diagnostics;
using RecipesProject.Models;
using System.Security.Claims;


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


		public IActionResult Index()
		{
			// Assuming you want to display the latest testimonial or any specific testimonial
			var testimonial = _context.Testimonials.FirstOrDefault(); // You may need to adjust this query based on your requirement

			return View(testimonial);
		}
		public IActionResult User()
		{
			// Assuming you want to display the latest testimonial or any specific testimonial
			var testimonial = _context.Testimonials.FirstOrDefault(); // You may need to adjust this query based on your requirement

			return View(testimonial);
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

        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Chefrecipe()
        {
            // Retrieve chefs with role ID 3 from the database asynchronously
            var chefs = await _context.Users.Where(u => u.Roleid == 2).ToListAsync();

            // Pass the list of chefs to the view
            return View(chefs);
            //    // Retrieve the logged-in chef's ID from claims
            //    var loggedInChefId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //    if (loggedInChefId == null)
            //    {
            //        // Handle case where user is not authenticated
            //        return RedirectToAction("Login", "Account"); // Redirect to login page
            //    }

            //    // Retrieve chefs from the database asynchronously
            //    var chefs = await _context.Users.Where(u => u.Roleid == 2).ToListAsync();

            //    // Pass the list of chefs and the logged-in chef's ID to the view
            //    ViewBag.LoggedInChefId = loggedInChefId;
        }
    
        public IActionResult Chefindex()
        {
            return View();
        }
        public async Task<IActionResult> Chef()
        {
            // Retrieve chefs with role ID 3 from the database asynchronously
            var chefs = await _context.Users.Where(u => u.Roleid == 2).ToListAsync();

            // Pass the list of chefs to the view
            return View(chefs);
        }

        public async Task<IActionResult> Recipe(decimal chefid, string category)
        {
            // Retrieve categories from the database asynchronously
            var categories = await _context.Recipecategories.Select(c => c.Categoryname).ToListAsync();
            ViewBag.Categories = categories;

            // Pass the categories and other necessary data to the view
            ViewBag.Categories = categories;

            // Retrieve recipes for the selected chef from the database asynchronously
            var recipesQuery = _context.Recipes
                                .Where(r => r.Chefid == chefid); // Capture chefId here
            //foreach (var Recipe in Recipes)
            //{
            //    if (Recipe.CategoryID.HasValue)
            //    {
            //        Recipe.CategoryName = categories.FirstOrDefault(c => c == Recipe.CategoryID.ToString()) ?? "Unknown";
            //    }
            //    else
            //    {
            //        Recipe.Categoryname = "Unknown";
            //    }
            //}
            // Retrieve recipes from the database asynchronously
            var recipes = await recipesQuery.ToListAsync();

            // Pass the list of recipes to the view
            return View(recipes);
        }

        public ActionResult Search(string searchTerm, string destination, string Category)
        {
            // Your logic to filter trips based on searchTerm, destination, and categoryName
            var filteredTrips = _context.Recipes.ToList();

           

            if (!string.IsNullOrEmpty(Category))
            {
                // Find the category ID based on the provided categoryName
                var categoryId = _context.Recipecategories
                    .Where(c => c.Categoryname == Category)
                    .Select(c => c.Categoryid)
                    .FirstOrDefault();

                // Filter trips based on the found category ID
                if (categoryId > 0)
                {
                    filteredTrips = filteredTrips.Where(t => t.Categoryid == categoryId).ToList();
                }
                else
                {
                    // Handle the case where the category name is not found
                    // You can redirect to an error page or handle it as needed
                    return RedirectToAction("Error");
                }
            }

            // Return the filtered trips to the view
            return View(filteredTrips);
        }


	

		//public IActionResult Testimonial()
		//{
		//    return View();
		//}

		//[HttpPost] // This attribute indicates that this action method should be invoked for HTTP POST requests
		//public IActionResult Submit(Testimonial testimonial)
		//{
		//    // Save the testimonial to the database
		//    // You need to implement the logic to save the testimonial to your database here

		//    // Redirect to a thank you page or show a success message
		//    return RedirectToAction("ThankYou");
		//}

	}
} 