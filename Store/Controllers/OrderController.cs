using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Models;

namespace Store.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly StoreContext _context;
    private readonly IWebHostEnvironment _environment;

    public OrderController(StoreContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }
    
    // GET
    public IActionResult Index()
    {
        List<Order> orders = _context.Orders.Include(o => o.Product).ToList();
        return View(orders);
    }
    
    //-----------------------------------
    //Create
    [Authorize]
    public IActionResult Create(int id)
    {
        Product a = _context.Products.FirstOrDefault(x => x.Id == id);
        ViewBag.Product = a;
        return View(); 
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create(Order order)
    {
        if (order != null)
        {
            Order o = new Order()
            {
                Name = order.Name,
                Address = order.Address,
                ContactPhone = order.ContactPhone,
                Quantity = order.Quantity,
                OrderDate = order.OrderDate,
                ProductId = order.ProductId
            };
            _context.Add(o);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return NotFound();
    }
}