using System.ComponentModel.DataAnnotations;

namespace Store.Models;

public class Brand
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Brand Name is required")]
    [StringLength(100,MinimumLength = 3, ErrorMessage = "The brand name must be between from 3 to 100 characters")]
    public string Name { get; set; }
    [Required]
    [StringLength(50,MinimumLength = 3,ErrorMessage = "The brand description must be between from 3 to 50 characters")]
    public string Description { get; set; }
}