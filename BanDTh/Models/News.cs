using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BanDTh.Models;

public partial class News
{
    public int NewsId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tiêu đề.")]
    public string Title { get; set; } = null!;

    public string? Slug { get; set; }

    public string? Thumbnail { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập nội dung.")]
    public string? Content { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn một tác giả.")]
    public int AuthorId { get; set; } // ✅ Validation ở đây là đúng

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn một danh mục.")]
    public int CategoryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsPublished { get; set; }
    public int? BrandId { get; set; }
    public virtual Brand? Brand { get; set; }

    // ✅ ĐẢM BẢO DÒNG NÀY KHÔNG CÓ [Required]
    public virtual User Author { get; set; } = null!;

    // ✅ TƯƠNG TỰ, DÒNG NÀY CŨNG KHÔNG CÓ [Required]
    public virtual Category? Category { get; set; }
}