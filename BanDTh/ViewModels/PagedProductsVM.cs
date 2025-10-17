using BanDTh.Models;
namespace BanDTh.ViewModels;
public class PagedProductsVM
{
    public List<Product> Items { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public string? Query { get; set; }
    public string? Category { get; set; }
    public string? Sort { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);
}
