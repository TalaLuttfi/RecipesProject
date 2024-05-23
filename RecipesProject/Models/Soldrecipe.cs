using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipesProject.Models;

public partial class Soldrecipe
{
    public decimal Soldrecipeid { get; set; }

    public decimal? Recipeid { get; set; }

    public decimal? Buyerid { get; set; }

    public DateTime? Purchasedate { get; set; }

    public virtual User? Buyer { get; set; }

    public virtual Recipe? Recipe { get; set; }

}
