using System;
using System.Collections.Generic;

namespace BanDTh.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int UserId { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Province { get; set; }

    public string? PostalCode { get; set; }

    public virtual User User { get; set; } = null!;
}
