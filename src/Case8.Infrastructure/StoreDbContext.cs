using Case8.Domain;
using Microsoft.EntityFrameworkCore;

namespace Case8.Infrastructure;

public class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<AdminNotification> AdminNotifications => Set<AdminNotification>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.HasIndex(x => x.Name).IsUnique();
        });
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(160).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(2000).IsRequired();
            entity.Property(x => x.Price).HasPrecision(18, 2);
            entity.Property(x => x.RowVersion).IsRowVersion();
            entity.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Cart>(entity => entity.HasIndex(x => x.SessionKey).IsUnique());
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasIndex(x => new { x.CartId, x.ProductId }).IsUnique();
            entity.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<OrderItem>(entity => entity.Property(x => x.UnitPrice).HasPrecision(18, 2));
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(x => x.Total).HasPrecision(18, 2);
            entity.Property(x => x.ShippingCost).HasPrecision(18, 2);
        });
        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Fresh Produce" },
            new Category { Id = 2, Name = "Bakery" },
            new Category { Id = 3, Name = "Pantry" });
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Organic Avocados", Description = "Creamy, ripe organic avocados for everyday meals.", Price = 89.90m, Stock = 28, IsActive = true, CategoryId = 1, ImageUrl = "/assets/images/product/product1.png" },
            new Product { Id = 2, Name = "Red Apples", Description = "Crisp seasonal apples selected from local growers.", Price = 64.50m, Stock = 40, IsActive = true, CategoryId = 1, ImageUrl = "/assets/images/product/product2.png" },
            new Product { Id = 3, Name = "Sourdough Bread", Description = "Freshly baked sourdough with a golden crust.", Price = 55.00m, Stock = 12, IsActive = true, CategoryId = 2, ImageUrl = "/assets/images/product/product3.png" },
            new Product { Id = 4, Name = "Raw Forest Honey", Description = "Natural honey with a rich, floral finish.", Price = 149.90m, Stock = 18, IsActive = true, CategoryId = 3, ImageUrl = "/assets/images/product/product4.png" },
            new Product { Id = 5, Name = "Baby Spinach", Description = "Tender washed spinach leaves for salads and cooking.", Price = 42.50m, Stock = 0, IsActive = true, CategoryId = 1, ImageUrl = "/assets/images/product/product5.png" },
            new Product { Id = 6, Name = "Whole Grain Granola", Description = "Oats, seeds and dried fruit in a crunchy pantry staple.", Price = 119.90m, Stock = 24, IsActive = true, CategoryId = 3, ImageUrl = "/assets/images/product/product6.png" });
    }
}
