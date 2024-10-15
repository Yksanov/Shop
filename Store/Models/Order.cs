using System.Runtime.InteropServices.JavaScript;

namespace Store.Models;

public class Order
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string ContactPhone { get; set; }
    public int Quantity { get; set; }
    public DateTime OrderDate { get; set; }
    
    public int? ProductId { get; set; }
    public Product? Product { get; set; }
}