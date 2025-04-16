using System;
using System.Collections.Generic;

namespace TheWag.Functions.EF;

public partial class Vendor
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
