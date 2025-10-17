using System;
using System.Collections.Generic;

namespace BanDTh.Models;

public partial class EmployeeKpi
{
    public int Kpiid { get; set; }

    public int UserId { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public int? TotalOrders { get; set; }

    public decimal? TotalRevenue { get; set; }

    public int? CustomerCount { get; set; }

    public decimal? SatisfactionScore { get; set; }

    public decimal? Bonus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
