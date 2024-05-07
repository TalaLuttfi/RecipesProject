using System;
using System.Collections.Generic;

namespace RecipesProject.Models;

public partial class Testimonial
{
    public decimal Testimonialid { get; set; }

    public decimal? Userid { get; set; }

    public string? Testimonialtext { get; set; }

    public string? Approvalstatus { get; set; }

    public virtual User? User { get; set; }
}
