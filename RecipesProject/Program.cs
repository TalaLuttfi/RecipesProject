using Microsoft.EntityFrameworkCore;
using RecipesProject.Models;
using System.Net.Mail;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace RecipesProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ModelContext>(x => x.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(60); });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.UseSession();
            app.Run();
        }

        public static void SendInvoiceEmail(string toEmail, string subject, string body, int recipeId, decimal recipePrice, DateTime paymentDate)
        {
            string fromMail = "lutfitala35@gmail.com";
            string fromPassword = "yjkj lggu fvrc cpse";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(toEmail));
            message.Body = body;
            message.IsBodyHtml = true;

            // Generate PDF invoice
            byte[] pdfBytes = GenerateInvoicePDF(recipeId, recipePrice, paymentDate);

            // Attach PDF to email
            MemoryStream ms = new MemoryStream(pdfBytes);
            message.Attachments.Add(new Attachment(ms, "Invoice.pdf", "application/pdf"));

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }

        public static void SendRecipeEmail(string toEmail, string subject, string body, byte[] recipePDF)
        {
            string fromMail = "lutfitala35@gmail.com";
            string fromPassword = "yjkj lggu fvrc cpse";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(toEmail));
            message.Body = body;
            message.IsBodyHtml = true;

            // Attach recipe PDF to email
            MemoryStream ms = new MemoryStream(recipePDF);
            message.Attachments.Add(new Attachment(ms, "Recipe.pdf", "application/pdf"));

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }

        public static byte[] GenerateInvoicePDF(int recipeId, decimal recipePrice, DateTime paymentDate)
        {
            MemoryStream ms = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, ms);
            document.Open();

            // Add content to PDF
            document.Add(new Paragraph("Invoice"));
            document.Add(new Paragraph("Recipe ID: " + recipeId));
            document.Add(new Paragraph("Amount Paid: $" + recipePrice));
            document.Add(new Paragraph("Payment Date: " + paymentDate.ToString()));

            document.Close();
            byte[] pdfBytes = ms.ToArray();
            return pdfBytes;
        }

        public static byte[] GenerateRecipePDF(Recipe recipe)
        {
            MemoryStream ms = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, ms);
            document.Open();

            // Add content to PDF
            document.Add(new Paragraph("Recipe Details"));
            document.Add(new Paragraph("Recipe Name: " + recipe.Recipename));
            document.Add(new Paragraph("Description: " + recipe.Description));
            document.Add(new Paragraph("Ingredients: " + recipe.Ingredients));
            document.Add(new Paragraph("Instructions: " + recipe.Instructions));

            document.Close();
            byte[] pdfBytes = ms.ToArray();
            return pdfBytes;
        }
    }
}
