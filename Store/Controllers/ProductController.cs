using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Store.Models;
using Store.Repository;
using Store.Services;
using Store.ViewModels;

namespace Store.Controllers;

public class ProductController : Controller
{
    private readonly StoreContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly IUserRepository _userRepository;

    public ProductController(StoreContext context, IWebHostEnvironment environment, IUserRepository userRepository)
    {
        _context = context;
        _environment = environment;
        _userRepository = userRepository;
    }

    // GET
    public async Task<IActionResult> Index(int? categoryId, int? brandId,
        SortProductState sortOrder = SortProductState.NameAsc, int page = 1)
    {
        IEnumerable<Product> product = await _context.Products.Include(p => p.Brand).ToListAsync();
        ViewBag.NameSort = sortOrder == SortProductState.NameAsc ? SortProductState.NameDesc : SortProductState.NameAsc;
        ViewBag.PriceSort = sortOrder == SortProductState.PriceAsc
            ? SortProductState.PriceDesc
            : SortProductState.PriceAsc;
        ViewBag.BrandSort = sortOrder == SortProductState.BrandAsc
            ? SortProductState.BrandDesc
            : SortProductState.BrandAsc;
        ViewBag.CategorySort = sortOrder == SortProductState.CategoryAsc
            ? SortProductState.CategoryDesc
            : SortProductState.CategoryAsc;
        ViewBag.CreatedDateSort = sortOrder == SortProductState.CreatedDateAsc
            ? SortProductState.CreatedDateDesc
            : SortProductState.CreatedDateAsc;
        switch (sortOrder)
        {
            case SortProductState.NameAsc:
                product = product.OrderBy(p => p.ProductName);
                break;
            case SortProductState.NameDesc:
                product = product.OrderByDescending(p => p.ProductName);
                break;
            case SortProductState.PriceAsc:
                product = product.OrderBy(p => p.Price);
                break;
            case SortProductState.PriceDesc:
                product = product.OrderByDescending(p => p.Price);
                break;
            case SortProductState.BrandAsc:
                product = product.OrderBy(p => p.Brand.Name);
                break;
            case SortProductState.BrandDesc:
                product = product.OrderByDescending(p => p.Brand.Name);
                break;
            case SortProductState.CategoryAsc:
                product = product.OrderBy(p => p.Category.Name);
                break;
            case SortProductState.CategoryDesc:
                product = product.OrderByDescending(p => p.Category.Name);
                break;
            case SortProductState.CreatedDateAsc:
                product = product.OrderBy(p => p.CreatedDate);
                break;
            case SortProductState.CreatedDateDesc:
                product = product.OrderByDescending(p => p.CreatedDate);
                break;
        }

        if (brandId.HasValue && brandId.Value != 0)
            product = product.Where(p => p.BrandId == brandId);
        if (categoryId.HasValue && categoryId.Value != 0)
            product = product.Where(p => p.CategoryId == categoryId);

        if (!product.Any())
        {
            ViewBag.Message = "Товары нету по вашему запросу!";
        }

        ViewBag.Categories = _context.Categories.ToList();
        ViewBag.Brands = _context.Brands.ToList();


        int pageSize = 3;
        var items = product.Skip((page - 1) * pageSize).Take(pageSize);
        PageViewModel pvm = new PageViewModel(product.Count(), page, pageSize);

        List<Brand> brands = await _context.Brands.ToListAsync();
        brands.Insert(0, new Brand() { Id = 0, Name = "All brands" });

        List<Category> categories = await _context.Categories.ToListAsync();
        categories.Insert(0, new Category() { Id = 0, Name = "All categories" });

        var vm = new ProductWithViewModel()
        {
            Products = items.ToList(),
            Brands = brands,
            Categories = categories,
            PageViewModel = pvm
        };
        return View(vm);
    }

    //----------------------------------------------------------
    //Create
    [Authorize(Roles = "admin")]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
        ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name");

        return View();
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public IActionResult Create([Bind("ProductName, Price, ImageUrl, CreatedDate, UpdatedDate, CategoryId, BrandId")] Product product)
    {
        if (ModelState.IsValid)
        {
            if (product != null)
            {
                product.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
                product.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
                
                _context.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        return View(product);
    }

    //----------------------------------------------------------
    // Edit
    [Authorize(Roles = "admin")]
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

    [Authorize(Roles = "admin")]
    [HttpPost]
    public IActionResult Edit(Product product)
    {
        if (product != null)
        {
            product.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
            _context.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(product);
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

        var obj = _context.Products.Find(id);
        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [Authorize(Roles = "admin")]
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
        List<Product> products = _context.Products
            .Include(c => c.Comments)
            .Include(p => p.Category)
            .Include(p => p.Brand).ToList();
        var findProduct = products.FirstOrDefault(p => p.Id == id);
        return View(findProduct);
    }

    
    //----------------------------------------------------------
    public IActionResult SearchProduct()
    {
        return View();
    }

    public async Task<IActionResult> SearchProductResults(string keyWord)
    {
        List<Product> results = await _context.Products.Include(p => p.Brand).Include(p => p.Category)
            .Where(p => p.ProductName.Contains(keyWord)).ToListAsync();
        return PartialView("SearchProductResultsPartialView", results);
    }

    [HttpPost]
    public async Task<IActionResult> NewComment(int? productId, string? name, string? body)
    {
        if (productId.HasValue && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(body))
        {
            Comment comment = new Comment()
            {
                Id = 0,
                ProductId = productId.Value,
                Name = name,
                Body = body
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        return Json("");
    }

    public async Task<IActionResult> GetComments(int? productId)
    {
        if (productId.HasValue)
        {
            List<Comment> comments = await _context.Comments.Include(c => c.Product).Where(p => p.ProductId == productId).ToListAsync();
            return PartialView("_CommentsPartialView", comments);
        }

        return Content("");
    }
}