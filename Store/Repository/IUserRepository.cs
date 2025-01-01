using System.Security.Claims;
using Store.Models;

namespace Store.Repository;

public interface IUserRepository
{
   public Task<MyUser?> GetUserAsync(ClaimsPrincipal user);
}