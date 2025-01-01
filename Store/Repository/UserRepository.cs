using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Store.Models;

namespace Store.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserManager<MyUser> _userManager;

    public UserRepository(UserManager<MyUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<MyUser?> GetUserAsync(ClaimsPrincipal user)
    {
        return await _userManager.GetUserAsync(user);
    }
}