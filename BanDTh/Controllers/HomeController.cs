using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BanDTh.Models;
using BanDTh.Data;
using Microsoft.AspNetCore.Identity;
using BanDTh.ViewModels;
using System.Security.Cryptography;
using System.Text;
using System.Reflection.Metadata;
using BanDTh.Helpers;
namespace BanDTh.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db)
    {
        _db = db;
    }

    // File: Controllers/HomeController.cs

    public async Task<IActionResult> Index()
    {
        var allProducts = await _db.Products
            .Include(p => p.Category)
            .Include(p => p.Promotions)
            .Where(p => p.Category != null)
            .OrderByDescending(p => p.ProductId)
            .ToListAsync();

        // ✅ SỬA ĐỔI: Gọi hàm GetPriceDetails và dùng ViewBag.PriceDetails
        var priceDetails = allProducts.ToDictionary(p => p.ProductId, p => PriceHelper.GetPriceDetails(p));
        ViewBag.PriceDetails = priceDetails;

        var productsByCategory = allProducts
            .GroupBy(p => p.Category!)
            .ToDictionary(g => g.Key, g => g.ToList());

        var vm = new HomeViewModel
        {
            Categories = await _db.Categories.OrderBy(c => c.CategoryId).ToListAsync(),
            ProductsByCategory = productsByCategory,
            FeaturedProducts = allProducts.Take(8).ToList()
        };

        return View(vm);
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    // POST: Login

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        // 🔹 1. Tìm user theo email, kèm Role
        var user = _db.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == email);

        // 🔹 2. Không tìm thấy user
        if (user == null)
        {
            ModelState.AddModelError("", "Sai email hoặc mật khẩu!");
            return View();
        }

        // 🔹 3. Kiểm tra tài khoản bị khóa
        if (!user.IsActive)
        {
            ModelState.AddModelError("", "Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên.");
            return View();
        }

        // 🔹 4. Kiểm tra mật khẩu
        if (!VerifyPassword(password, user.PasswordHash))
        {
            ModelState.AddModelError("", "Sai email hoặc mật khẩu!");
            return View();
        }

        // 🔹 5. Lưu Session
        HttpContext.Session.SetString("UserEmail", user.Email ?? string.Empty);
        HttpContext.Session.SetString("UserName", user.FullName ?? string.Empty);
        HttpContext.Session.SetString("UserRole", user.Role?.RoleName ?? string.Empty);

        // 🔹 6. Điều hướng theo Role
        switch (user.Role?.RoleName)
        {
            case "Admin":
                return RedirectToAction("Index", "Admin");
            case "Sales":
                return RedirectToAction("Index", "Sales");
            case "Warehouse":
                return RedirectToAction("Index", "Warehouse");
            default:
                return RedirectToAction("Index", "Home");
        }
    }
    private bool VerifyPassword(string inputPassword, string storedHash)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var hashBytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(inputPassword));
        var hashString = Convert.ToHexString(hashBytes);
        return string.Equals(hashString, storedHash, StringComparison.OrdinalIgnoreCase);
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    // POST: Register
    [HttpPost]
    public IActionResult Register(User model, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            ModelState.AddModelError("", "Mật khẩu xác nhận không khớp!");
            return View(model);
        }

        if (_db.Users.Any(u => u.Email == model.Email))
        {
            ModelState.AddModelError("", "Email đã tồn tại!");
            return View(model);
        }

        // Hash mật khẩu
        model.PasswordHash = HashPassword(password);
        model.RoleId = 2; // Mặc định là Customer

        _db.Users.Add(model);
        _db.SaveChanges();

        return RedirectToAction("Login");
    }

    // Removed duplicate Index action to resolve method conflict
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("UserRole");
        HttpContext.Session.Remove("UserName");
        HttpContext.Session.Remove("UserEmail");
        return RedirectToAction("Index", "Home");
    }
    public IActionResult Profile(int id)
    {
        var user = _db.Users.FirstOrDefault(u => u.UserId == id);
        return View(user); // User
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
