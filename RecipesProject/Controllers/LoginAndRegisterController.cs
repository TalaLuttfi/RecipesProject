using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RecipesProject.Models;

namespace RecipesProject.Controllers
{
	public class LoginAndRegisterController : Controller
	{
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment
              _hostEnvironment;
        public LoginAndRegisterController(ModelContext context, IWebHostEnvironment _hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = _hostEnvironment;


        }
        public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Login(User user)
		{
			var auth = _context.Users.FirstOrDefault(x => x.Username == user.Username && x.Password == user.Password);
			if (auth != null)
			{
				// Store username, userid, and email in session
				HttpContext.Session.SetString("Username", auth.Username);
				HttpContext.Session.SetInt32("Userid", Convert.ToInt32(auth.Userid));
				HttpContext.Session.SetString("UserEmail", auth.Email); // Store user email in session

				switch (auth.Roleid)
				{
					case 1:
						return RedirectToAction("Index", "Admin");
					case 2:
						return RedirectToAction("Chefindex", "Home");
					case 3:
						return RedirectToAction("User", "Home");

					// Handle other roles here

					default:
						ModelState.AddModelError(string.Empty, "Invalid role.");
						return View();
				}
			}
			else
			{
				TempData["ErrorMessage"] = "Username or password is wrong.";
				return View();
			}
		}



		public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Userid,Username,Email,Password,Imagepath,ImageFile,Roleid")] User user)
        {
            if (ModelState.IsValid)
            {
                //Add Customer Details
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await user.ImageFile.CopyToAsync(filestream);

                }
                user.Imagepath = fileName;

                if (user.Roleid == 2) // Guest selected "Chef"
                {
                    user.Roleid = 2;
                }
                else if (user.Roleid == 3) // Guest selected "User"
                {
                    user.Roleid = 3;
                }
                else
                {
                    // Default role or handle other cases
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "LoginAndRegister");
            }
            return View(user);
        }



    }
}

