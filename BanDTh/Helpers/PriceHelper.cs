// File: Helpers/PriceHelper.cs
using BanDTh.Models;
using System;
using System.Linq;

namespace BanDTh.Helpers
{
    public static class PriceHelper
    {
        // Đổi tên hàm và kiểu trả về
        public static PriceCalculationResult GetPriceDetails(Product product)
        {
            var now = DateTime.Now.Date;

            if (product == null)
            {
                return new PriceCalculationResult { FinalPrice = 0, PromotionEndDate = null };
            }

            var activePromotions = product.Promotions
                .Where(p => p.IsActive == true &&
                            p.StartDate.Value.Date <= now &&
                            p.EndDate.Value.Date >= now)
                .ToList();

            if (!activePromotions.Any())
            {
                // Không có khuyến mãi, trả về giá gốc
                return new PriceCalculationResult { FinalPrice = product.Price, PromotionEndDate = null };
            }

            var bestPromotion = activePromotions
                .OrderByDescending(p => p.DiscountType == "Percent")
                .ThenByDescending(p => p.DiscountValue)
                .FirstOrDefault();

            if (bestPromotion == null)
            {
                return new PriceCalculationResult { FinalPrice = product.Price, PromotionEndDate = null };
            }

            decimal finalPrice;
            if (bestPromotion.DiscountType == "Percent")
            {
                finalPrice = product.Price - (product.Price * (bestPromotion.DiscountValue.Value / 100));
            }
            else // "Amount"
            {
                finalPrice = product.Price - bestPromotion.DiscountValue.Value;
            }

            // Trả về cả giá cuối cùng và ngày kết thúc
            return new PriceCalculationResult
            {
                FinalPrice = finalPrice,
                PromotionEndDate = bestPromotion.EndDate
            };
        }
    }
}