using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Models;

namespace Store.Controllers;

public class CategoryController : Controller
{
    private readonly StoreContext _context;
    private readonly IWebHostEnvironment _environment;
    
    public CategoryController(StoreContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }
    
    // GET
    [Authorize(Roles = "admin, user")]
    public IActionResult Index()
    {
        List<Category> categories = _context.Categories.ToList();
        return View(categories);
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
    public IActionResult Create(Category category)
    {
        string errorMessage = null;
        if (_context.Categories.Any(c => c.Name == category.Name))
        {
            errorMessage = $"{category.Name} категория с таким именем уже существует!";
            ViewBag.ErrorMessage = errorMessage;
            return View(category);
        }
        if (category != null)
        {
            _context.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(category);
    }
    //----------------------------------------------------------
    
    //Delete
    [Authorize(Roles = "admin")]
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var obj = _context.Categories.Find(id);
        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public IActionResult Delete(Category category)
    {
        if (category != null)
        {
            _context.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(category);
    }
}