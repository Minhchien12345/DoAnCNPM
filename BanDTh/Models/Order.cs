using System;
using System.Collections.Generic;

namespace BanDTh.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public string? OrderCode { get; set; }

    public int? TotalProducts { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public string? ShippingMethod { get; set; }

    public string? PaymentMethod { get; set; }

    public string? ReceiverName { get; set; }

    public string? ReceiverPhone { get; set; }

    public string? Address { get; set; }

    public string? Note { get; set; }

    public bool? Instruction { get; set; }

    public int? RewardPoints { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? DiscountId { get; set; }

    public virtual ICollection<CustomerFeedback> CustomerFeedbacks { get; set; } = new List<CustomerFeedback>();

    public virtual DiscountCode? Discount { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<RewardHistory> RewardHistories { get; set; } = new List<RewardHistory>();

    public virtual User User { get; set; } = null!;
}
