using Case8.Application;
using Case8.Domain;
using Microsoft.EntityFrameworkCore;

namespace Case8.Infrastructure;

public class StorefrontService(StoreDbContext db) : IStorefrontService
{
    public async Task<IReadOnlyList<ProductListItem>> GetFeaturedProductsAsync() => await ProductItems(db.Products.Where(x => x.IsActive).OrderBy(x => x.Name)).Take(6).ToListAsync();
    public async Task<IReadOnlyList<CategoryItem>> GetCategoriesAsync() => await db.Categories.OrderBy(x => x.Name).Select(x => new CategoryItem(x.Id, x.Name)).ToListAsync();

    public async Task<ShopResult> GetShopAsync(string? search, int? categoryId, string? sort, int page, int pageSize)
    {
        var query = db.Products.AsNoTracking().Where(x => x.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(x => x.Name.Contains(search));
        if (categoryId.HasValue) query = query.Where(x => x.CategoryId == categoryId.Value);
        query = sort == "price_asc" ? query.OrderBy(x => x.Price) : sort == "price_desc" ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Name);
        var total = await query.CountAsync();
        var products = await ProductItems(query.Skip((page - 1) * pageSize).Take(pageSize)).ToListAsync();
        return new ShopResult(products, await GetCategoriesAsync(), total, page, pageSize);
    }

    public async Task<ProductDetail?> GetProductAsync(int id) => await db.Products.AsNoTracking().Where(x => x.Id == id && x.IsActive).Select(x => new ProductDetail(x.Id, x.Name, x.Description, x.Price, x.Stock, x.Category.Name, x.ImageUrl)).FirstOrDefaultAsync();
    public async Task<CartSummary> GetCartAsync(string sessionKey)
    {
        var items = await db.CartItems.AsNoTracking().Where(x => x.Cart.SessionKey == sessionKey).Select(x => new CartLine(x.ProductId, x.Product.Name, x.Product.Price, x.Quantity, x.Product.Stock, x.Product.IsActive, x.Product.ImageUrl)).ToListAsync();
        return new CartSummary(items, items.Sum(x => x.UnitPrice * x.Quantity));
    }

    public async Task AddToCartAsync(string sessionKey, int productId, int quantity)
    {
        if (quantity < 1) return;
        var product = await db.Products.SingleOrDefaultAsync(x => x.Id == productId && x.IsActive);
        if (product is null || product.Stock == 0) return;
        var cart = await db.Carts.Include(x => x.Items).SingleOrDefaultAsync(x => x.SessionKey == sessionKey);
        if (cart is null) { cart = new Cart { SessionKey = sessionKey }; db.Carts.Add(cart); }
        var line = cart.Items.SingleOrDefault(x => x.ProductId == productId);
        if (line is null) cart.Items.Add(new CartItem { ProductId = productId, Quantity = Math.Min(quantity, product.Stock) });
        else line.Quantity = Math.Min(line.Quantity + quantity, product.Stock);
        await db.SaveChangesAsync();
    }

    public async Task UpdateCartAsync(string sessionKey, int productId, int quantity)
    {
        var line = await db.CartItems.Include(x => x.Product).SingleOrDefaultAsync(x => x.Cart.SessionKey == sessionKey && x.ProductId == productId);
        if (line is null) return;
        if (quantity < 1) db.CartItems.Remove(line); else line.Quantity = Math.Min(quantity, line.Product.Stock);
        await db.SaveChangesAsync();
    }

    public async Task RemoveFromCartAsync(string sessionKey, int productId)
    {
        var line = await db.CartItems.SingleOrDefaultAsync(x => x.Cart.SessionKey == sessionKey && x.ProductId == productId);
        if (line is null) return;
        db.CartItems.Remove(line);
        await db.SaveChangesAsync();
    }

    private static IQueryable<ProductListItem> ProductItems(IQueryable<Product> query) => query.Select(x => new ProductListItem(x.Id, x.Name, x.Price, x.Stock, x.Category.Name, x.ImageUrl));
}
