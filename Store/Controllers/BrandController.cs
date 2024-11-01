using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Models;
using Store.ViewModels;

namespace Store.Controllers;

[Authorize]
public class BrandController : Controller
{
    private readonly StoreContext _context;
    private readonly IWebHostEnvironment _environment;
    
    public BrandController(StoreContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }
    
    // GET
    [Authorize(Roles = "admin, user")]
    public async Task<IActionResult> Index(int page = 1)
    {
        List<Brand> brands = await _context.Brands.ToListAsync();
        int pageSize = 3;
        var count = brands.Count();
        var items = brands.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
        BrandPageViewModel bpvm = new BrandPageViewModel()
        {
            PageViewModel = pageViewModel,
            Brands = items
        };
        
        return View(bpvm);

        // List<Brand> brands = _context.Brands.ToList();
        // return View(brands);
    }
    //----------------------------------------------------------
    //Create
    [Authorize(Roles = "admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Brand brand)
    {
        string errorMessage = null;
        if (_context.Brands.Any(c => c.Name == brand.Name))
        {
            errorMessage = $"{brand.Name} бренд с таким именем уже существует!";
            ViewBag.ErrorMessage = errorMessage;
            return View(brand);
        }
        
        if (brand != null)
        {
            _context.Add(brand);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(brand);
    }
    //----------------------------------------------------------
    
    //Delete
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var obj = _context.Brands.Find(id);
        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost]
    public IActionResult Delete(Brand brand)
    {
        if (brand != null)
        {
            _context.Remove(brand);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(brand);
    }
}