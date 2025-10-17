using System;
using System.Collections.Generic;

namespace BanDTh.Models;

public partial class DiscountCode
{
    public int DiscountId { get; set; }

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public decimal DiscountValue { get; set; }

    public string? DiscountType { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? Status { get; set; }

    public decimal? MinOrderValue { get; set; }

    public decimal? MaxDiscount { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
