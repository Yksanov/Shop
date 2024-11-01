using Store.Models;

namespace Store.ViewModels;

public class BrandPageViewModel
{
    public PageViewModel PageViewModel { get; set; }
    public List<Brand> Brands { get; set; }
}