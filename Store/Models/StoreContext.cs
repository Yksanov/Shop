using Microsoft.EntityFrameworkCore;

namespace Store.Models;

public class StoreContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }
}