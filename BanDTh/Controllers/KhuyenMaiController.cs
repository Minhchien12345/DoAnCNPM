using BanDTh.Data;
using BanDTh.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class KhuyenMaiController : Controller
{
    private readonly AppDbContext _context;

    public KhuyenMaiController(AppDbContext context)
    {
        _context = context;
    }

    [Route("/khuyen-mai")]
    public async Task<IActionResult> Index()
    {
        var now = DateTime.Now.Date;

        var promotionalProducts = await _context.Products
            .Include(p => p.Promotions)
            .Where(p => p.Promotions.Any(promo =>
                promo.IsActive == true &&
                promo.StartDate.Value.Date <= now &&
                promo.EndDate.Value.Date >= now))
            .OrderByDescending(p => p.ProductId)
            .ToListAsync();

        // ✅ SỬA ĐỔI: Gọi hàm GetPriceDetails và dùng ViewBag.PriceDetails
        var priceDetails = promotionalProducts.ToDictionary(p => p.ProductId, p => PriceHelper.GetPriceDetails(p));
        ViewBag.PriceDetails = priceDetails;

        return View(promotionalProducts);
    }
}