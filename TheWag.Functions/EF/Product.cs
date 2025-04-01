using System;
using System.Collections.Generic;

namespace TheWag.Api.WagDB.EF;

public partial class Product
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string Description { get; set; } = null!;

    public int? FkVendorId { get; set; }

    public virtual Vendor? FkVendor { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
