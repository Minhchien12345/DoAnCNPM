using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BanDTh.Models;
using BanDTh.Data;
using Microsoft.AspNetCore.Identity;
using BanDTh.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace BanDTh.Controllers;

public class AdminController : Controller
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db)
    {
        _db = db;
    }
    [HttpGet]
    public IActionResult Index()
    {
        var dashboardData = new
        {
            UserCount = _db.Users.Count(),
            CategoryCount = _db.Categories.Count(),
            ProductCount = _db.Products.Count(),
            NewsCount = _db.News.Count(),
            PromotionCount = _db.Promotions.Count(),
            DiscountCodeCount = _db.DiscountCodes.Count(),
            RepairCount = _db.RepairItems.Count()
        };

        return View(dashboardData);
    }
    [HttpGet]
    // ✅ Hiển thị + tìm kiếm user
    public IActionResult Users(string keyword, int? roleFilter)
    {
        var users = _db.Users.Include(u => u.Role).AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
            users = users.Where(u => u.FullName.Contains(keyword) || u.Email.Contains(keyword));

        if (roleFilter.HasValue)
            users = users.Where(u => u.RoleId == roleFilter);

        ViewBag.Roles = _db.Roles.ToList();
        return View(users.ToList());
    }
    [HttpGet]

    public IActionResult CreateUser() => View();

    [HttpPost]
    public IActionResult CreateUser(string FullName, string Email, string Gender, string Phone, string Password, string ConfirmPassword, int RoleId)
    {
        if (Password != ConfirmPassword)
        {
            TempData["Error"] = "Mật khẩu xác nhận không khớp!";
            return RedirectToAction("Users");
        }

        using var sha = System.Security.Cryptography.SHA256.Create();
        var hash = Convert.ToHexString(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password)));

        var user = new User
        {
            FullName = FullName,
            Email = Email,
            UserName = Email,   // ✅ THÊM DÒNG NÀY
            Gender = Gender,
            Phone = Phone,
            PasswordHash = hash,
            RoleId = RoleId,
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        TempData["Success"] = "Tạo tài khoản thành công!";
        return RedirectToAction("Users");
    }
    [HttpGet]

    public IActionResult EditUser(int id)
    {
        var user = _db.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == id);
        if (user == null) return NotFound();
        ViewBag.Roles = _db.Roles.ToList();
        return View(user);
    }

    public IActionResult HideUser(int id)
    {
        var user = _db.Users.Find(id);
        if (user != null)
        {
            user.IsActive = false;
            _db.SaveChanges();
        }
        return RedirectToAction("Users");
    }

    [HttpPost]
    public IActionResult EditUser(User model)
    {
        if (ModelState.IsValid)
        {
            _db.Users.Update(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(model);
    }
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public IActionResult ToggleUserStatus(int id)
    {
        var user = _db.Users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
            return NotFound();

        user.IsActive = !user.IsActive;
        _db.SaveChanges();

        return Json(new { isActive = user.IsActive });
    }

}
