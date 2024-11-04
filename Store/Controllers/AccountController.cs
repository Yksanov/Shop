using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Models;
using Store.ViewModels;

namespace Store.Controllers;

public class AccountController : Controller
{
    private readonly StoreContext _context;

    public AccountController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            MyUser user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
            if (user != null)
            {
                await AuthenticateAsync(user); // аутентификация
                return RedirectToAction("Index", "Product");
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }

        return View(model);
    }
    
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Product");
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            MyUser user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                Role r = _context.Roles.FirstOrDefault(u => u.Name == "user");
                MyUser myUser = new MyUser() { Email = model.Email, Password = model.Password,UserName = model.UserName, Role = r, RoleId = r.Id};
                await _context.Users.AddAsync(myUser);
                await _context.SaveChangesAsync();
                await AuthenticateAsync(myUser);
                return RedirectToAction("Index", "Product");
            }
            ModelState.AddModelError(string.Empty, "Пользователь с таким Email уже зарегистрирован!");
        }
        return View(model);
    }
    
    
    public async Task AuthenticateAsync(MyUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name),
            new Claim("UserName", user.UserName)
        };
        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id),
            new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            });
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}