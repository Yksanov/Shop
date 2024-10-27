using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace Store.Models;

public class Order
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Order Name is required")]
    [StringLength(100,MinimumLength = 3, ErrorMessage = "The order name must be between from 3 to 100 characters")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Address is required")]
    [StringLength(50,MinimumLength = 3, ErrorMessage = "The order address must be between from 3 to 50 characters")]
    public string Address { get; set; }
    [Required(ErrorMessage = "Phone is required")]
    [StringLength(50,MinimumLength = 3, ErrorMessage = "The order phone must be between from 3 to 50 characters")]
    public string ContactPhone { get; set; }
    [Required]
    [Range(1,50, ErrorMessage = "Quantity must be from 1 to 50 quantity")]
    public int Quantity { get; set; }
    public DateOnly OrderDate { get; set; }
    
    public int? ProductId { get; set; }
    public Product? Product { get; set; }
}