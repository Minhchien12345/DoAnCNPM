// File: Controllers/ProductsApiController.cs

using BanDTh.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[ApiController]
[Route("api/products")]
public class ProductsApiController : ControllerBase
{
    private readonly AppDbContext _context;

    // ✅ SỬA ĐỔI QUAN TRỌNG: Dùng Dependency Injection, không dùng "new AppDbContext()"
    public ProductsApiController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /api/products
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .ToListAsync();

        return Ok(products); // IActionResult hoạt động tốt
    }

    // GET: /api/products/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }
}