using System;
using System.Collections.Generic;
using TheWag.Api.WagDB.EF;

namespace TheWag.Api.WagDB.EF;

public partial class Order
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int FkCustomerId { get; set; }

    public virtual Customer IdNavigation { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
