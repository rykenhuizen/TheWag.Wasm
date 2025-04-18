﻿using System;
using System.Collections.Generic;

namespace TheWag.Functions.EF;

public partial class Customer
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
