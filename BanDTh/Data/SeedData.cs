using System.Security.Cryptography;
using System.Text;
using BanDTh.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace BanDTh.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        // Đảm bảo database đã được tạo
        context.Database.Migrate();

        // 1️⃣ Seed Brand
        if (!context.Brands.Any())
        {
            context.Brands.AddRange(
                new Brand { Name = "Apple", Slug = "apple", Logo = "apple-logo.png", Description = "Thương hiệu Mỹ nổi tiếng với iPhone, MacBook, iPad." },
                new Brand { Name = "Samsung", Slug = "samsung", Logo = "samsung-logo.png", Description = "Hãng điện tử Hàn Quốc với dòng Galaxy và SmartTV." },
                new Brand { Name = "Asus", Slug = "asus", Logo = "asus-logo.png", Description = "Thương hiệu laptop, linh kiện máy tính Đài Loan." }
            );
            context.SaveChanges();
        }

        // 2️⃣ Seed Role
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { RoleName = "Admin", RoleDescription = "Quản trị hệ thống, có toàn quyền" },
                new Role { RoleName = "Customer", RoleDescription = "Khách hàng sử dụng website để mua hàng" },
                new Role { RoleName = "Sales", RoleDescription = "Nhân viên bán hàng, xử lý đơn hàng và hỗ trợ khách" },
                new Role { RoleName = "Warehouse", RoleDescription = "Nhân viên kho, quản lý hàng tồn và xuất nhập kho" }
            );
            context.SaveChanges();
        }

        // 3️⃣ Seed User
        if (!context.Users.Any())
        {
            // Hàm mã hoá mật khẩu (SHA256)
            string HashPassword(string password)
            {
                using var sha = System.Security.Cryptography.SHA256.Create();
                var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToHexString(bytes);
            }

            var adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
            var salesRole = context.Roles.FirstOrDefault(r => r.RoleName == "Sales");
            var customerRole = context.Roles.FirstOrDefault(r => r.RoleName == "Customer");

            if (adminRole == null || salesRole == null || customerRole == null)
            {
                throw new Exception("Roles chưa được seed. Hãy đảm bảo bảng Roles có dữ liệu trước khi seed Users.");
            }

            var users = new List<User>
    {
        new User
        {
            FullName = "Admin",
            UserName = "admin",
            Email = "admin@gmail.com",
            PasswordHash = HashPassword("admin123"),
            RoleId = adminRole.RoleId,
            IsActive = true,
            CreatedAt = DateTime.Now
        },
        new User
        {
            FullName = "Nhân viên bán hàng",
            UserName = "sales",
            Email = "sales@gmail.com",
            PasswordHash = HashPassword("sales123"),
            RoleId = salesRole.RoleId,
            IsActive = true,
            CreatedAt = DateTime.Now
        },
        new User
        {
            FullName = "Khách hàng",
            UserName = "customer",
            Email = "customer@gmail.com",
            PasswordHash = HashPassword("customer123"),
            RoleId = customerRole.RoleId,
            IsActive = true,
            CreatedAt = DateTime.Now
        }
    };

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        // 4️⃣ Seed Category
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Iphone", Slug = "i-phone", Icon = "fa-mobile" },
                new Category { Name = "Điện thoại", Slug = "dien-thoai", Icon = "fa-mobile" },
                new Category { Name = "Tai Nghe", Slug = "tai-Nghe", Icon = "fa-laptop" },
                new Category { Name = "Phụ kiện", Slug = "phu-kien", Icon = "fa-headphones" }
            );
            context.SaveChanges();
        }

        // 5️⃣ Seed Product
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product
                {
                    Name = "iPhone 15 Pro Max",
                    Slug = "iphone-15-pro-max",
                    Description = "Flagship Apple năm 2025, chip A17 Pro, camera 48MP, Titanium Frame",
                    Price = 36990000,
                    OldPrice = 39990000,
                    Stock = 100,
                    Thumbnail = "iphone15promax.jpg",
                    CategoryId = 1,
                    BrandId = 1
                },
                new Product
                {
                    Name = "Samsung Galaxy S24 Ultra",
                    Slug = "samsung-galaxy-s24-ultra",
                    Description = "Flagship Samsung, hỗ trợ bút S-Pen, màn hình 120Hz",
                    Price = 32990000,
                    OldPrice = 35990000,
                    Stock = 80,
                    Thumbnail = "s24ultra.jpg",
                    CategoryId = 1,
                    BrandId = 2
                },
                new Product
                {
                    Name = "Asus Zenbook Pro 14",
                    Slug = "asus-zenbook-pro-14",
                    Description = "Laptop cao cấp dành cho designer và lập trình viên.",
                    Price = 27990000,
                    OldPrice = 29990000,
                    Stock = 60,
                    Thumbnail = "zenbookpro14.jpg",
                    CategoryId = 2,
                    BrandId = 3
                }
            );
            context.SaveChanges();
        }

        // 6️⃣ Seed DiscountCode
        if (!context.DiscountCodes.Any())
        {
            context.DiscountCodes.AddRange(
                new DiscountCode
                {
                    Code = "TOPZONE10",
                    Description = "Giảm 10% cho đơn đầu tiên",
                    DiscountType = "Percent",
                    DiscountValue = 10,
                    ExpiryDate = DateTime.Now.AddMonths(1),
                    Status = "Hoạt động",
                    MinOrderValue = 1000000
                },
                new DiscountCode
                {
                    Code = "SHIPFREE",
                    Description = "Miễn phí vận chuyển toàn quốc",
                    DiscountType = "Amount",
                    DiscountValue = 50000,
                    ExpiryDate = DateTime.Now.AddMonths(3),
                    Status = "Hoạt động"
                }
            );
            context.SaveChanges();
        }
        // 🧑‍💼 Seed Users
        if (!context.Users.Any())
        {
            var adminRole = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
            var customerRole = context.Roles.FirstOrDefault(r => r.RoleName == "Customer");
            // Hàm mã hóa SHA256
            static string HashPassword(string password)
            {
                using (var sha256 = SHA256.Create())
                {
                    var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    return BitConverter.ToString(bytes).Replace("-", "").ToLower();
                }
            }

            var users = new List<User>
                {
                    new User
                    {
                        UserName = "aaadmin",
                        PasswordHash = HashPassword("123456"), // trong thực tế nên mã hóa
                        FullName = "Quản Trị Viên",
                        Gender = "Nam",
                        Email = "admin@bandth.vn",
                        Phone = "0909009009",
                        RoleId = adminRole.RoleId,
                        CreatedAt = DateTime.Now
                    },
                    new User
                    {
                        UserName = "customer",
                        PasswordHash = HashPassword("123456"),
                        FullName = "Khách Hàng Mẫu",
                        Gender = "Nữ",
                        Email = "customer@bandth.vn",
                        Phone = "0911223344",
                        RoleId = customerRole.RoleId,
                        CreatedAt = DateTime.Now
                    }
                }
            ;
            context.Users.AddRange(users);
            context.SaveChanges();
        }

        Console.WriteLine("✅ Seed dữ liệu mẫu hoàn tất.");
    }

}

