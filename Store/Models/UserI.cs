using Microsoft.AspNetCore.Identity;

namespace Store.Models;

public class UserI : IdentityUser<int>
{
    public int Age { get; set; }
}