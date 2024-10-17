using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Models;

namespace Store.Controllers;

public class ProductController : Controller
{
    private readonly StoreContext _context;
    private readonly IWebHostEnvironment _environment;

    public ProductController(StoreContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }
    
    // GET
    public IActionResult Index(int? categoryId, int? brandId)
    {
        List<Product> products = _context.Products.ToList();
        if (categoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
        }
        if (brandId.HasValue)
        {
            products = products.Where(p => p.BrandId == brandId.Value).ToList();
        }

        if (!products.Any())
        {
            ViewBag.Message = "Товары нету по вашему запросу!";
        }

        ViewBag.Categories = _context.Categories.ToList();
        ViewBag.Brands = _context.Brands.ToList();
        
        return View(products);
    }
    
    //----------------------------------------------------------
    //Create
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
        ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name");
        
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (product != null)
        {
            _context.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(product);
    }
    
    //----------------------------------------------------------
    // Edit
    public IActionResult Edit(int id)
    {
        Product p = _context.Products.FirstOrDefault(x => x.Id == id);
        if (p == null)
        {
            return NotFound($"Product with this id: {id} not found");
        }
        
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
        ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name");
        
        return View(p);
    }

    [HttpPost]
    public IActionResult Edit(Product product)
    {
        if (product != null)
        {
            _context.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(product);
    }
    
    //----------------------------------------------------------
    //Delete
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var obj = _context.Products.Find(id);
        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost]
    public IActionResult Delete(Product product)
    {
        if (product != null)
        {
            _context.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(product);
    }
    //----------------------------------------------------------
    //Details
    [HttpGet]
    public IActionResult Details(int id)
    {
        List<Product> products = _context.Products.ToList();
        var findProduct = products.FirstOrDefault(p => p.Id == id);
        return View(findProduct);
    }
}