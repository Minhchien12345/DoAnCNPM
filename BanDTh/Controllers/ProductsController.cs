// File: Controllers/ProductsController.cs
using BanDTh.Data;
using BanDTh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[Route("admin/products")]
public class ProductsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // --- HIỂN THỊ DANH SÁCH ---
    [Route("")]
    [Route("index")]
    public async Task<IActionResult> Index()
    {
        var products = await _context.Products
            .Include(p => p.Category) // Lấy kèm tên danh mục
            .Include(p => p.Brand)    // Lấy kèm tên thương hiệu
            .OrderByDescending(p => p.ProductId)
            .ToListAsync();
        return View(products);
    }

    // --- THÊM MỚI ---
    [Route("create")]
    public IActionResult Create()
    {
        // Chuẩn bị dữ liệu cho các dropdown
        ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name");
        ViewBag.Brands = new SelectList(_context.Brands, "BrandId", "Name");
        return View();
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Slug,Description,Price,OldPrice,Stock,CategoryId,BrandId")] Product product, IFormFile? thumbnailFile)
    {
        ModelState.Remove("Category");
        ModelState.Remove("Brand");

        if (ModelState.IsValid)
        {
            if (thumbnailFile != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + thumbnailFile.FileName;
                string filePath = Path.Combine(uploadsDir, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbnailFile.CopyToAsync(fileStream);
                }
                product.Thumbnail = uniqueFileName;
            }
            product.CreatedAt = DateTime.Now;

            _context.Add(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Thêm sản phẩm mới thành công!";
            return RedirectToAction(nameof(Index));
        }

        // Nếu lỗi, tải lại dropdown
        ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
        ViewBag.Brands = new SelectList(_context.Brands, "BrandId", "Name", product.BrandId);
        return View(product);
    }

    // --- SỬA ---
    [Route("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
        ViewBag.Brands = new SelectList(_context.Brands, "BrandId", "Name", product.BrandId);
        return View(product);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Slug,Description,Price,OldPrice,Stock,Thumbnail,CategoryId,BrandId,CreatedAt")] Product product, IFormFile? thumbnailFile)
    {
        if (id != product.ProductId) return NotFound();

        ModelState.Remove("Category");
        ModelState.Remove("Brand");

        if (ModelState.IsValid)
        {
            if (thumbnailFile != null)
            {
                if (!string.IsNullOrEmpty(product.Thumbnail))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/products", product.Thumbnail);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + thumbnailFile.FileName;
                string filePath = Path.Combine(uploadsDir, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbnailFile.CopyToAsync(fileStream);
                }
                product.Thumbnail = uniqueFileName;
            }

            _context.Update(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
        ViewBag.Brands = new SelectList(_context.Brands, "BrandId", "Name", product.BrandId);
        return View(product);
    }

    // --- XÓA ---
    [Route("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.ProductId == id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost("delete/{id:int}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            if (!string.IsNullOrEmpty(product.Thumbnail))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/products", product.Thumbnail);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Xóa sản phẩm thành công!";
        }
        return RedirectToAction(nameof(Index));
    }
}