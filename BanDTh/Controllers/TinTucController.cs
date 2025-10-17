// File: Controllers/TinTucController.cs

using BanDTh.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class TinTucController : Controller
{
    private readonly AppDbContext _context;

    public TinTucController(AppDbContext context)
    {
        _context = context;
    }

    // --- HIỂN THỊ TRANG DANH SÁCH TIN TỨC (ĐÃ NÂNG CẤP VỚI BỘ LỌC) ---
    [Route("/tin-tuc")]
    public async Task<IActionResult> Index(DateTime? searchDate, int? categoryId, int? brandId)
    {
        // Lấy query cơ bản
        var query = _context.News
            .Where(n => n.IsPublished)
            .Include(n => n.Author)
            .AsQueryable(); // Chuyển sang AsQueryable để có thể thêm điều kiện lọc

        // Áp dụng các bộ lọc nếu có
        if (searchDate.HasValue)
        {
            query = query.Where(n => n.CreatedAt.HasValue && n.CreatedAt.Value.Date == searchDate.Value.Date);
        }
        if (categoryId.HasValue && categoryId > 0)
        {
            query = query.Where(n => n.CategoryId == categoryId);
        }
        if (brandId.HasValue && brandId > 0)
        {
            query = query.Where(n => n.BrandId == brandId);
        }

        // Thực thi query và lấy danh sách tin tức đã lọc
        var newsList = await query.OrderByDescending(n => n.CreatedAt).ToListAsync();

        // Chuẩn bị dữ liệu cho các dropdown trên form lọc
        ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name", categoryId);
        ViewBag.Brands = new SelectList(await _context.Brands.ToListAsync(), "BrandId", "Name", brandId);

        // Giữ lại giá trị ngày đã lọc để hiển thị lại trên form
        ViewBag.CurrentDate = searchDate?.ToString("yyyy-MM-dd");

        return View(newsList);
    }

    // --- HIỂN THỊ TRANG CHI TIẾT MỘT BÀI VIẾT (giữ nguyên) ---
    [Route("/tin-tuc/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var newsArticle = await _context.News
            .Include(n => n.Author)
            .Include(n => n.Category)
            .Include(n => n.Brand) // Thêm Include Brand để hiển thị nếu cần
            .FirstOrDefaultAsync(n => n.Slug == slug && n.IsPublished);

        if (newsArticle == null)
        {
            return NotFound();
        }

        return View(newsArticle);
    }
}