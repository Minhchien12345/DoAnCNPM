// File: ViewModels/PromotionViewModel.cs
using BanDTh.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BanDTh.ViewModels
{
    public class PromotionViewModel
    {
        // Kế thừa các thuộc tính từ model Promotion
        public int PromotionId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề.")]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá trị giảm giá.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 0.")]
        public decimal DiscountValue { get; set; }

        public string DiscountType { get; set; } = "Percent";

        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày kết thúc.")]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        // Thuộc tính đặc biệt để nhận danh sách ID sản phẩm được chọn từ form
        [Required(ErrorMessage = "Vui lòng chọn ít nhất một sản phẩm.")]
        public List<int> SelectedProductIds { get; set; } = new List<int>();
    }
}