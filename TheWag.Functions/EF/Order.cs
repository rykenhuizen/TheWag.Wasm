using System;
using System.Collections.Generic;

namespace TheWag.Functions.EF;

public partial class Order
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int FkCustomerId { get; set; }

    public decimal? Total { get; set; }

    public virtual Customer FkCustomer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
