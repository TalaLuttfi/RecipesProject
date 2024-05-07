using System;
using System.Collections.Generic;

namespace RecipesProject.Models;

public partial class Contactu
{
    public decimal Contactusid { get; set; }

    public string? Message { get; set; }

    public decimal? Phonenumber { get; set; }

    public string? Senderemail { get; set; }
}
