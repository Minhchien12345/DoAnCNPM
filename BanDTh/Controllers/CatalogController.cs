using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BanDTh.Data;
using BanDTh.ViewModels;
using BanDTh.Helpers;

namespace BanDTh.Controllers;

public class CatalogController : Controller
{
    private readonly AppDbContext _db;
    public CatalogController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? category, string? q, int page = 1, int pageSize = 12, string sort = "popular")
    {
        var query = _db.Products
            .Include(p => p.Category)
            .Include(p => p.Promotions)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => p.Category!.Slug == category);
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(p => p.Name.Contains(q));

        query = sort switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            _ => query.OrderByDescending(p => p.ProductId)
        };

        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        // ✅ SỬA ĐỔI: Gọi hàm GetPriceDetails và dùng ViewBag.PriceDetails
        var priceDetails = items.ToDictionary(p => p.ProductId, p => PriceHelper.GetPriceDetails(p));
        ViewBag.PriceDetails = priceDetails;

        var vm = new PagedProductsVM
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            Total = total,
            Query = q,
            Category = category,
            Sort = sort
        };
        return View(vm);
    }

    [Route("catalog/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var product = await _db.Products
            .Include(p => p.Category)
            .Include(p => p.Promotions)
            .FirstOrDefaultAsync(p => p.Slug == slug);

        if (product == null) return NotFound();

        // ✅ SỬA ĐỔI: Gọi hàm GetPriceDetails và dùng ViewBag.PriceDetails
        var priceDetails = PriceHelper.GetPriceDetails(product);
        ViewBag.PriceDetails = priceDetails;

        return View(product);
    }

    public IActionResult Featured()
    {
        var products = _db.Products.Include(p => p.Promotions).ToList();

        // ✅ SỬA ĐỔI: Gọi hàm GetPriceDetails và dùng ViewBag.PriceDetails
        var priceDetails = products.ToDictionary(p => p.ProductId, p => PriceHelper.GetPriceDetails(p));
        ViewBag.PriceDetails = priceDetails;

        return View(products);
    }
}