using System;
using System.Collections.Generic;

namespace TheWag.Functions.EF;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int FkOrderId { get; set; }

    public int FkProductId { get; set; }

    public int Qty { get; set; }

    public virtual Order FkOrder { get; set; } = null!;

    public virtual Product FkProduct { get; set; } = null!;
}
