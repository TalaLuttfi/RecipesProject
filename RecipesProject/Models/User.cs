using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipesProject.Models;

public partial class User
{
    public decimal Userid { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public decimal? Roleid { get; set; }

    public string? Imagepath { get; set; }

    public string? Description { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Soldrecipe> Soldrecipes { get; set; } = new List<Soldrecipe>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
