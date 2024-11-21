using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Models;
using Store.ViewModels;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace Store.Controllers;

public class AccountController : Controller
{
    //private readonly StoreContext _context;
    private readonly UserManager<UserI> _userManager;
    private readonly SignInManager<UserI> _signInManager;

    public AccountController(UserManager<UserI> userManager, SignInManager<UserI> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;

        //_context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        ViewBag.ReturnUrl = HttpContext.Request.Query["ReturnUrl"];
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            UserI user = await _userManager.FindByEmailAsync(model.Email);
            Microsoft.AspNetCore.Identity.SignInResult signInResult =
                await _signInManager.PasswordSignInAsync(user, model.Password, model.RememerMe, false);
            if (signInResult.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index", "Product");
            }
            ViewBag.ReturnUrl = HttpContext.Request.Query["ReturnUrl"];
            ModelState.AddModelError(string.Empty, "Error");
        }

        return View(model);

        // if (ModelState.IsValid)
        // {
        //     MyUser user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
        //     if (user != null)
        //     {
        //         await AuthenticateAsync(user); // аутентификация
        //         return RedirectToAction("Index", "Product");
        //     }
        //     ModelState.AddModelError(String.Empty, "Некорректные логин и(или) пароль");
        // }
        //
        // return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await _signInManager.SignOutAsync();
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
            UserI user = new UserI()
            {
                Email = model.Email,
                UserName = model.Email,
                Age = model.Age
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                await _userManager.AddToRoleAsync(user, "user");
                return RedirectToAction("Index", "Product");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);


        // if (ModelState.IsValid)
        // {
        //     MyUser user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
        //     if (user == null)
        //     {
        //         //Role r = _context.Roles.FirstOrDefault(u => u.Name == "user");
        //         MyUser myUser = new MyUser() { Email = model.Email, Password = model.Password,UserName = model.UserName, Role};
        //         await _context.Users.AddAsync(myUser);
        //         await _context.SaveChangesAsync();
        //         await AuthenticateAsync(myUser);
        //         return RedirectToAction("Index", "Product");
        //     }
        //     ModelState.AddModelError(string.Empty, "Пользователь с таким Email уже зарегистрирован!");
        // }
        // return View(model);
    }


    // public async Task AuthenticateAsync(MyUser user)
    // {
    //     var claims = new List<Claim>
    //     {
    //         new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
    //         new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
    //     };
    //
    //     if (user.UserName != null)
    //         claims.Add(new Claim("UserName", user.UserName));
    //     
    //     ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
    //     await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id),
    //         new AuthenticationProperties()
    //         {
    //             IsPersistent = true,
    //             ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
    //         });
    // }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}