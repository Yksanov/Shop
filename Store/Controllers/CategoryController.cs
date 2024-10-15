using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
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
    public IActionResult Index()
    {
        List<Category> categories = _context.Categories.ToList();
        return View(categories);
    }
    //----------------------------------------------------------
    //Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category category)
    {
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