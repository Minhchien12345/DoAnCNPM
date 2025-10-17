using System;
using System.Collections.Generic;

namespace BanDTh.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? DiscountValue { get; set; }

    public string? DiscountType { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
