using System;
using System.Collections.Generic;

namespace TheWag.Api.WagDB.EF;

public partial class Tag
{
    public int Id { get; set; }

    public int FkProductId { get; set; }

    public string Text { get; set; } = null!;

    public virtual Product FkProduct { get; set; } = null!;
}
