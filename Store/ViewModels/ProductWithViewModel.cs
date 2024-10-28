using Store.Models;

namespace Store.ViewModels;

public class ProductWithViewModel
{
    public IEnumerable<Product> Products { get; set; }
    public IEnumerable<Brand> Brands { get; set; }
    public IEnumerable<Category> Categories { get; set; }
    public PageViewModel PageViewModel { get; set; }
}