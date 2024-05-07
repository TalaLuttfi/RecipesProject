using Microsoft.AspNetCore.Mvc;

namespace RecipesProject.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
