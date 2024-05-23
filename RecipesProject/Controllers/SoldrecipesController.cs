using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipesProject.Models;
using System.Globalization;

using RecipesProject.Models; // Import EmailService namespace
using Microsoft.Extensions.Configuration; // Import IConfiguration namespace
using System.Net.Mail; // Import SmtpClient namespace

using System.Net;
using System.Net.Mail;
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
        // Modify the Index action in your 
        // Modify the Index action in your controller
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
          
            IQueryable<Soldrecipe> modelContext = _context.Soldrecipes.Include(s => s.Buyer).Include(s => s.Recipe);

            // Filter by start and end dates if provided
            if (startDate != null && endDate != null)
            {
                modelContext = modelContext.Where(s => s.Purchasedate >= startDate && s.Purchasedate <= endDate);
            }

            return View(await modelContext.ToListAsync());
        }



        // Action to generate monthly report
        public async Task<IActionResult> MonthlyReport(int month, int year)
        {
            var modelContext = _context.Soldrecipes.Include(s => s.Buyer).Include(s => s.Recipe)
                                  .Where(s => s.Purchasedate.Value.Month == month && s.Purchasedate.Value.Year == year);
            return View("Index", await modelContext.ToListAsync());
        }

        // Action to generate annual report
        public async Task<IActionResult> AnnualReport(int year)
        {
            var modelContext = _context.Soldrecipes.Include(s => s.Buyer).Include(s => s.Recipe)
                                  .Where(s => s.Purchasedate.Value.Year == year);
            return View("Index", await modelContext.ToListAsync());
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Recipeid,Buyerid,Purchasedate")] Soldrecipe soldrecipe, Payment payment, Visacard visa)
        {
            if (ModelState.IsValid)
            {
                int recipeId = (int)Convert.ToDecimal(HttpContext.Request.Form["RecipeId"]);
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

                    // Retrieve user email from session
                    var userEmail = HttpContext.Session.GetString("UserEmail");

                    // Create invoice email body
                    string invoiceEmailBody = $@"
                <html>
                <body>
                    <h1>Invoice</h1>
                    <p>Thank you for your purchase!</p>
                    <p>Recipe ID: {recipeId}</p>
                    <p>Amount Paid: ${recipePrice}</p>
                    <p>Payment Date: {payment.Paymentdate}</p>
                </body>
                </html>";

                    // Send invoice email
                    Program.SendInvoiceEmail(userEmail, "Your Invoice", invoiceEmailBody, recipeId, recipePrice, payment.Paymentdate.Value);

                    // Retrieve recipe details
                    decimal recipeIdDecimal = Convert.ToDecimal(recipeId);
                    var recipe = await _context.Recipes.FindAsync(recipeIdDecimal);

                    // Check if recipe exists
                    if (recipe != null)
                    {
                        // Create recipe email body
                        string recipeEmailBody = $@"
        <html>
        <body>
            <h1>Recipe Details</h1>
            <p>Recipe Name: {recipe.Recipename}</p>
            <p>Description: {recipe.Description}</p>
            <p>Ingredients: {recipe.Ingredients}</p>
            <p>Instructions: {recipe.Instructions}</p>
        </body>
        </html>";

                        // Generate PDF of recipe details
                        byte[] recipePDF = Program.GenerateRecipePDF(recipe);

                        // Send recipe email with PDF attachment
                        Program.SendRecipeEmail(userEmail, "Your Recipe", recipeEmailBody, recipePDF);

                        TempData["SuccessMessage"] = "Payment successful!";
                        return RedirectToAction("BuyRecipe", "Home"); // Redirect to a suitable page after payment
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Recipe not found.";
                        return RedirectToAction("Index", "Home"); // Redirect to the home page if recipe is not found
                    }

                }
                else
                {
                    TempData["ErrorMessage"] = "Insufficient balance or invalid card number.";
                }
            }

            return View(soldrecipe); // Return the view with the model in case of error
        }


        //         try
        //            {
        //                using (var mail = new MailMessage())
        //                {
        //                    mail.From = new MailAddress("lutfitala35@gmail.com", "yjkj lggu fvrc cpse");
        //        mail.To.Add(new MailAddress(userEmail)); // Use the user's email address
        //                    mail.Subject = "Invoice for Recipe Purchase";
        //                    mail.Body = $"Thank you for your purchase! The total amount paid is {payment.Amount:C}.";

        //                    using (var smtp = new SmtpClient("smtp.ethereal.email", 587))
        //                    {
        //                        smtp.Credentials = new NetworkCredential("lutfitala35@gmail.com", "yjkj lggu fvrc cpse");
        //        smtp.EnableSsl = true; // Ethereal SMTP server requires STARTTLS

        //                        await smtp.SendMailAsync(mail);
        //    }
        //}
        //            }
        //            catch (Exception ex)
        //            {
        //    // Handle exception (log, display error message, etc.)
        //    Console.WriteLine($"An error occurred while sending email: {ex.Message}");
        //}

        //private async Task SendInvoiceEmailAsync(Payment payment, string userEmail)
        //{
        //    string fromMail = "lutfitala35@gmail.com";
        //    string fromPassword = "yjkj lggu fvrc cpse"; // Use an App password if 2-step verification is enabled

        //    MailMessage message = new MailMessage();
        //    message.From = new MailAddress(fromMail, "Tala");
        //    message.Subject = "Invoice for Recipe Purchase";
        //    message.To.Add(new MailAddress(userEmail)); // Use the user's email address
        //    message.Body = $"<html><body>Thank you for your purchase! The total amount paid is {payment.Amount:C}.</body></html>";
        //    message.IsBodyHtml = true;

        //    using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
        //    {
        //        smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword);
        //        smtpClient.EnableSsl = true;

        //        try
        //        {
        //            await smtpClient.SendMailAsync(message);
        //        }
        //        catch (SmtpException ex)
        //        {
        //            // Handle the exception (e.g., log it, display an error message, etc.)
        //            Console.WriteLine($"An error occurred while sending email: {ex.Message}");
        //            throw; // Re-throw the exception if you want to handle it further up the call stack
        //        }
        //    }
        //}



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
