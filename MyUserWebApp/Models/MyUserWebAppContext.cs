using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace MyUserWebApp.Models;

public class MyUserWebAppContext : IdentityDbContext<MyUser>
{  
    public MyUserWebAppContext(DbContextOptions<MyUserWebAppContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    //DbSet<CartItem> CartItems { get; set; }
    //DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
