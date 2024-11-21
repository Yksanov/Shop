using System.ComponentModel.DataAnnotations;

namespace Store.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Age is required")]
    public int Age { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d@!%*?&]{8,50}$", ErrorMessage = "Password must be between 8 and 50 characters long, contain at least one uppercase letter and one digit")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }
    
    [Required(ErrorMessage = "User Name is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "User Name cannot be longer than 50 characters")]
    public string? UserName { get; set; }
}