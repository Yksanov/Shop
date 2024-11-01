using Microsoft.EntityFrameworkCore;

namespace Store.Models;

public class StoreContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<MyUser> Users { get; set; }
    
    public DbSet<Role> Roles { get; set; }
    
    public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(new Role {Id = 1, Name = "admin"});
        modelBuilder.Entity<Role>().HasData(new Role {Id = 2, Name = "user"});
        modelBuilder.Entity<MyUser>().HasData(new MyUser {Id = 2, Email = "admin@admin.com", Password = "1qwe@QWE", RoleId = 1});
        base.OnModelCreating(modelBuilder);
    }
}