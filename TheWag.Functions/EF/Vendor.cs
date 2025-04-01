using System;
using System.Collections.Generic;

namespace TheWag.Api.WagDB.EF;

public partial class Vendor
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
