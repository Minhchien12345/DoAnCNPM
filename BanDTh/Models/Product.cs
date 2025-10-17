using System;
using System.Collections.Generic;

namespace BanDTh.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Slug { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal? OldPrice { get; set; }

    public int? Stock { get; set; }

    public string? Thumbnail { get; set; }

    public int CategoryId { get; set; }

    public int? BrandId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<InventoryStatistic> InventoryStatistics { get; set; } = new List<InventoryStatistic>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<RepairItem> RepairItems { get; set; } = new List<RepairItem>();

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}
