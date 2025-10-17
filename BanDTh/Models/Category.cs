using System;
using System.Collections.Generic;

namespace BanDTh.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Slug { get; set; }

    public string? Icon { get; set; }

    // ✅ DÒNG ĐƯỢC THÊM VÀO
    public string? Description { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}