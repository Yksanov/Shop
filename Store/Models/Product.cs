using System.ComponentModel.DataAnnotations;

namespace Store.Models;

public class Product
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(100,MinimumLength = 3, ErrorMessage = "The product name must be between from 3 to 100 characters")]
    public string ProductName { get; set; }
    [Required]
    [Range(50, 100000, ErrorMessage = "The price should be from 50 to 100,000")]
    public int Price { get; set; }
    [Required(ErrorMessage = "Product image is required")]
    public string ImageUrl { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly UpdatedDate { get; set; }
    
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public int BrandId { get; set; }
    public Brand? Brand { get; set; }
}