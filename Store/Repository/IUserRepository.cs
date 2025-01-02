using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Store.Models;

namespace Store.Repository;

public interface IUserRepository
{
   public Task<MyUser?> GetUserAsync(ClaimsPrincipal user);
   public Task<MyUser?> GetUserByIdAsync(string userId);
   public Task<IdentityResult?> CreateUserAsync(MyUser user, string password);
   public Task<IdentityResult?> UpdateUserAsync(MyUser user);
   public Task<bool> IsInRoleAsync(MyUser user, string role);
   public Task<MyUser?> FindByEmailAsync(string email);
   public Task<MyUser?> FindByNameAsync(string userName);
   public Task<IdentityResult?> AddRoleAsync(MyUser user, string role);
}