using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Store.Models;

public class StoreContext : IdentityDbContext<UserI, IdentityRole<int>, int>
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    // public DbSet<MyUser> Users { get; set; }
    
    // public DbSet<Role> Roles { get; set; }
    
    public DbSet<Comment> Comments { get; set; }
    
    public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Brand>().HasMany(b => b.Products).WithOne(p => p.Brand).HasForeignKey();  List<Product> Products {get; set;}
        
        // modelBuilder.Entity<Role>().HasData(new Role {Id = 1, Name = "admin"});
        // modelBuilder.Entity<Role>().HasData(new Role {Id = 2, Name = "user"});
        // modelBuilder.Entity<MyUser>().HasData(new MyUser {Id = 2, Email = "admin@admin.com", Password = "1qwe@QWE", UserName = "admin", RoleId = 1});
        
        base.OnModelCreating(modelBuilder);
    }
}