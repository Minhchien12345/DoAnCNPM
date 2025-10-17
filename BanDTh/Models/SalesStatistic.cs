using System;
using System.Collections.Generic;

namespace BanDTh.Models;

public partial class SalesStatistic
{
    public int StatisticId { get; set; }

    public int UserId { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int? TotalOrders { get; set; }

    public int? TotalProducts { get; set; }

    public decimal? TotalRevenue { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
