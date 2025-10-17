// File: /Controllers/NewsController.cs

using BanDTh.Data;
using BanDTh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

// Route prefix cho tất cả các action trong controller này (ví dụ: /admin/news/...)
[Route("admin/news")]
public class NewsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    // Sử dụng Dependency Injection để lấy AppDbContext và IWebHostEnvironment
    public NewsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // --- 1. HIỂN THỊ DANH SÁCH (READ) ---
    [Route("")]
    [Route("index")]
    public async Task<IActionResult> Index()
    {
        var newsList = await _context.News
            .Include(n => n.Author)
            .Include(n => n.Category)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
        return View(newsList);
    }

    // --- 2. THÊM MỚI (CREATE) ---
    [Route("create")]
    public IActionResult Create()
    {
        ViewData["Authors"] = new SelectList(_context.Users, "UserId", "FullName");
        ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "Name");
        // ✅ SỬA ĐỔI: Gửi danh sách thương hiệu sang View
        ViewData["Brands"] = new SelectList(_context.Brands, "BrandId", "Name");
        return View();
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    // ✅ SỬA ĐỔI: Thêm BrandId vào thuộc tính Bind
    public async Task<IActionResult> Create([Bind("Title,Slug,Content,AuthorId,CategoryId,BrandId,IsPublished")] News news, IFormFile? thumbnailFile)
    {
        ModelState.Remove("Author");
        ModelState.Remove("Category");
        // ✅ SỬA ĐỔI: Bỏ qua validation cho đối tượng Brand
        ModelState.Remove("Brand");

        if (ModelState.IsValid)
        {
            if (thumbnailFile != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/news");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + thumbnailFile.FileName;
                string filePath = Path.Combine(uploadsDir, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbnailFile.CopyToAsync(fileStream);
                }
                news.Thumbnail = uniqueFileName;
            }

            news.CreatedAt = DateTime.Now;
            _context.Add(news);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Thêm tin tức thành công!";
            return RedirectToAction(nameof(Index));
        }

        ViewData["Authors"] = new SelectList(_context.Users, "UserId", "FullName", news.AuthorId);
        ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "Name", news.CategoryId);
        // ✅ SỬA ĐỔI: Gửi lại danh sách thương hiệu nếu có lỗi
        ViewData["Brands"] = new SelectList(_context.Brands, "BrandId", "Name", news.BrandId);
        return View(news);
    }

    // --- 3. CHỈNH SỬA (UPDATE) ---
    [Route("edit/{id:int}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var news = await _context.News.FindAsync(id);
        if (news == null) return NotFound();

        ViewData["Authors"] = new SelectList(_context.Users, "UserId", "FullName", news.AuthorId);
        ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "Name", news.CategoryId);
        // ✅ SỬA ĐỔI: Gửi danh sách thương hiệu sang View, chọn sẵn giá trị cũ
        ViewData["Brands"] = new SelectList(_context.Brands, "BrandId", "Name", news.BrandId);
        return View(news);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    // ✅ SỬA ĐỔI: Thêm BrandId vào thuộc tính Bind
    public async Task<IActionResult> Edit(int id, [Bind("NewsId,Title,Slug,Content,Thumbnail,AuthorId,CategoryId,BrandId,IsPublished,CreatedAt")] News news, IFormFile? thumbnailFile)
    {
        if (id != news.NewsId) return NotFound();

        ModelState.Remove("Author");
        ModelState.Remove("Category");
        // ✅ SỬA ĐỔI: Bỏ qua validation cho đối tượng Brand
        ModelState.Remove("Brand");

        if (ModelState.IsValid)
        {
            try
            {
                if (thumbnailFile != null)
                {
                    if (!string.IsNullOrEmpty(news.Thumbnail))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/news", news.Thumbnail);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/news");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + thumbnailFile.FileName;
                    string filePath = Path.Combine(uploadsDir, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbnailFile.CopyToAsync(fileStream);
                    }
                    news.Thumbnail = uniqueFileName;
                }

                _context.Update(news);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật tin tức thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.News.Any(e => e.NewsId == news.NewsId)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["Authors"] = new SelectList(_context.Users, "UserId", "FullName", news.AuthorId);
        ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "Name", news.CategoryId);
        // ✅ SỬA ĐỔI: Gửi lại danh sách thương hiệu nếu có lỗi
        ViewData["Brands"] = new SelectList(_context.Brands, "BrandId", "Name", news.BrandId);
        return View(news);
    }

    // --- 4. XÓA (DELETE) ---
    [Route("delete/{id:int}")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var news = await _context.News
            .Include(n => n.Author)
            .Include(n => n.Category)
            .FirstOrDefaultAsync(m => m.NewsId == id);
        if (news == null) return NotFound();

        return View(news);
    }

    [HttpPost("delete/{id:int}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var news = await _context.News.FindAsync(id);
        if (news != null)
        {
            if (!string.IsNullOrEmpty(news.Thumbnail))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/news", news.Thumbnail);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Xóa tin tức thành công!";
        }

        return RedirectToAction(nameof(Index));
    }
}