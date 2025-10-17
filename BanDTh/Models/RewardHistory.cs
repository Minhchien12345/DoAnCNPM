using System;
using System.Collections.Generic;

namespace BanDTh.Models;

public partial class RewardHistory
{
    public int RewardId { get; set; }

    public int UserId { get; set; }

    public int? OrderId { get; set; }

    public int Points { get; set; }

    public string? Type { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Order? Order { get; set; }

    public virtual User User { get; set; } = null!;
}
