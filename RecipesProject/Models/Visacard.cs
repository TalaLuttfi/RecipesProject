using System;
using System.Collections.Generic;

namespace RecipesProject.Models;

public partial class Visacard
{
    public decimal Cardid { get; set; }

    public string? Cardnumber { get; set; }

    public string? Holdername { get; set; }

    public DateTime? Expirydate { get; set; }

    public string? Cvv { get; set; }

    public decimal? Balance { get; set; }
}
