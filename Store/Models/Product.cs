namespace Store.Models;

public class Product
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public int Price { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public int BrandId { get; set; }
    public Brand? Brand { get; set; }
}