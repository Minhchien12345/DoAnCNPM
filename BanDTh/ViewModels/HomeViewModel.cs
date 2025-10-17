namespace BanDTh.Models
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; } = new();
        public List<Product> FeaturedProducts { get; set; } = new();
        public Dictionary<Category, List<Product>> ProductsByCategory { get; set; } = new();
    }
}