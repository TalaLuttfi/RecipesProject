using System;
using System.Collections.Generic;

namespace RecipesProject.Models;

public partial class Recipe
{
    public decimal Recipeid { get; set; }

    public decimal? Chefid { get; set; }

    public decimal? Categoryid { get; set; }

    public string? Recipename { get; set; }

    public string? Description { get; set; }

    public string? Ingredients { get; set; }

    public string? Instructions { get; set; }

    public string? Imagepath { get; set; }

    public decimal? Price { get; set; }

    public string? Approvalstatus { get; set; }

    public virtual Recipecategory? Category { get; set; }

    public virtual User? Chef { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Soldrecipe> Soldrecipes { get; set; } = new List<Soldrecipe>();
}
