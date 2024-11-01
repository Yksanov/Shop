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

    [Authorize(Roles = "admin")]
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
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var brand = await _context.Brands.FindAsync(id);
        if (brand == null)
        {
            return NotFound();
        }
        return View(brand);
    }
    
    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Brand brand)
    {
        if (id != brand.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(brand);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandExists(brand.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
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
    
    private bool BrandExists(int id)
    {
        return _context.Brands.Any(e => e.Id == id);
    }
}