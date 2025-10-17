using System;
using System.Collections.Generic;

namespace BanDTh.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int RoleId { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<CustomerFeedback> CustomerFeedbacks { get; set; } = new List<CustomerFeedback>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<EmployeeKpi> EmployeeKpis { get; set; } = new List<EmployeeKpi>();

    public virtual ICollection<InventoryStatistic> InventoryStatistics { get; set; } = new List<InventoryStatistic>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<RewardHistory> RewardHistories { get; set; } = new List<RewardHistory>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<SalesStatistic> SalesStatistics { get; set; } = new List<SalesStatistic>();

    public virtual ICollection<SystemLog> SystemLogs { get; set; } = new List<SystemLog>();
}

