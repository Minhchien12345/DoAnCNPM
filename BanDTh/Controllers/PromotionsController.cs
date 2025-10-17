// File: Controllers/PromotionsController.cs
using BanDTh.Data;
using BanDTh.Models;
using BanDTh.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[Route("admin/promotions")]
public class PromotionsController : Controller
{
    private readonly AppDbContext _context;

    public PromotionsController(AppDbContext context)
    {
        _context = context;
    }

    // --- HIỂN THỊ DANH SÁCH ---
    [Route("")]
    [Route("index")]
    public async Task<IActionResult> Index()
    {
        var promotions = await _context.Promotions
            .OrderByDescending(p => p.StartDate)
            .ToListAsync();
        return View(promotions);
    }

    // --- THÊM MỚI ---
    [Route("create")]
    public IActionResult Create()
    {
        ViewBag.MinDate = DateTime.Now.ToString("yyyy-MM-dd");
        ViewBag.Products = new MultiSelectList(_context.Products, "ProductId", "Name");
        return View(new PromotionViewModel());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PromotionViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var promotion = new Promotion
            {
                Title = vm.Title,
                Description = vm.Description,
                DiscountType = vm.DiscountType,
                DiscountValue = vm.DiscountValue,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                IsActive = vm.IsActive
            };

            // ✅ SỬA ĐỔI QUAN TRỌNG: Thêm promotion vào context TRƯỚC
            _context.Promotions.Add(promotion);

            // Sau đó mới tìm và gán các sản phẩm liên quan
            if (vm.SelectedProductIds.Any())
            {
                var selectedProducts = await _context.Products
                    .Where(p => vm.SelectedProductIds.Contains(p.ProductId)).ToListAsync();

                // Gán danh sách sản phẩm vào khuyến mãi đã được context theo dõi
                promotion.Products = selectedProducts;
            }

            // Lưu tất cả thay đổi (bao gồm cả khuyến mãi và các liên kết)
            await _context.SaveChangesAsync();
            TempData["Success"] = "Tạo chương trình khuyến mãi thành công!";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.MinDate = DateTime.Now.ToString("yyyy-MM-dd");
        ViewBag.Products = new MultiSelectList(_context.Products, "ProductId", "Name", vm.SelectedProductIds);
        return View(vm);
    }

    // --- SỬA (EDIT) ---
    [Route("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.MinDate = DateTime.Now.ToString("yyyy-MM-dd");

        var promotion = await _context.Promotions
            .Include(p => p.Products)
            .FirstOrDefaultAsync(p => p.PromotionId == id);

        if (promotion == null) return NotFound();

        var vm = new PromotionViewModel
        {
            PromotionId = promotion.PromotionId,
            Title = promotion.Title,
            Description = promotion.Description,
            DiscountType = promotion.DiscountType,
            DiscountValue = promotion.DiscountValue ?? 0,
            StartDate = promotion.StartDate.Value,
            EndDate = promotion.EndDate.Value,
            IsActive = promotion.IsActive ?? false,
            SelectedProductIds = promotion.Products.Select(p => p.ProductId).ToList()
        };

        ViewBag.Products = new MultiSelectList(_context.Products, "ProductId", "Name", vm.SelectedProductIds);
        return View(vm);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PromotionViewModel vm)
    {
        if (id != vm.PromotionId) return NotFound();

        if (ModelState.IsValid)
        {
            var promotionToUpdate = await _context.Promotions
                .Include(p => p.Products)
                .FirstOrDefaultAsync(p => p.PromotionId == id);

            if (promotionToUpdate == null) return NotFound();

            promotionToUpdate.Title = vm.Title;
            promotionToUpdate.Description = vm.Description;
            promotionToUpdate.DiscountType = vm.DiscountType;
            promotionToUpdate.DiscountValue = vm.DiscountValue;
            promotionToUpdate.StartDate = vm.StartDate;
            promotionToUpdate.EndDate = vm.EndDate;
            promotionToUpdate.IsActive = vm.IsActive;

            promotionToUpdate.Products.Clear();
            if (vm.SelectedProductIds.Any())
            {
                var selectedProducts = await _context.Products
                    .Where(p => vm.SelectedProductIds.Contains(p.ProductId)).ToListAsync();
                promotionToUpdate.Products = selectedProducts;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật khuyến mãi thành công!";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.MinDate = DateTime.Now.ToString("yyyy-MM-dd");
        ViewBag.Products = new MultiSelectList(_context.Products, "ProductId", "Name", vm.SelectedProductIds);
        return View(vm);
    }

    // --- XÓA (DELETE) ---
    [Route("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var promotion = await _context.Promotions
            .FirstOrDefaultAsync(m => m.PromotionId == id);
        if (promotion == null) return NotFound();
        return View(promotion);
    }

    [HttpPost("delete/{id:int}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var promotion = await _context.Promotions
            .Include(p => p.Products)
            .FirstOrDefaultAsync(p => p.PromotionId == id);

        if (promotion != null)
        {
            promotion.Products.Clear();
            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Xóa khuyến mãi thành công!";
        }

        return RedirectToAction(nameof(Index));
    }
}