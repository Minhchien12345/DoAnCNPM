using System;
using System.Collections.Generic;

namespace BanDTh.Models;

public partial class InventoryStatistic
{
    public int InventoryId { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int? QuantityIn { get; set; }

    public int? QuantityOut { get; set; }

    public int? StockRemaining { get; set; }

    public string? Note { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
