using System;
using System.Collections.Generic;

namespace BanDTh.Models;

public partial class RepairItem
{
    public int RepairItemId { get; set; }

    public int ProductId { get; set; }

    public int RepairCategoryId { get; set; }

    public int? RepairAccessoryId { get; set; }

    public string RepairName { get; set; } = null!;

    public string? Content { get; set; }

    public decimal Price { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual RepairAccessory? RepairAccessory { get; set; }

    public virtual RepairCategory RepairCategory { get; set; } = null!;
}
