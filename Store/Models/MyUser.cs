namespace Store.Models;

public class MyUser
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? UserName { get; set; }
    
    public int? RoleId { get; set; }
    public Role? Role { get; set; }
}