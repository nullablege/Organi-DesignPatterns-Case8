namespace Case8.Application;

public record ProductListItem(int Id, string Name, decimal Price, int Stock, string CategoryName, string? ImageUrl);
public record ProductDetail(int Id, string Name, string Description, decimal Price, int Stock, string CategoryName, string? ImageUrl);
public record CategoryItem(int Id, string Name);
public record ShopResult(IReadOnlyList<ProductListItem> Products, IReadOnlyList<CategoryItem> Categories, int TotalCount, int Page, int PageSize);
public record CartLine(int ProductId, string Name, decimal UnitPrice, int Quantity, int Stock, bool IsActive, string? ImageUrl);
public record CartSummary(IReadOnlyList<CartLine> Items, decimal Total);

public interface IStorefrontService
{
    Task<IReadOnlyList<ProductListItem>> GetFeaturedProductsAsync();
    Task<IReadOnlyList<CategoryItem>> GetCategoriesAsync();
    Task<ShopResult> GetShopAsync(string? search, int? categoryId, string? sort, int page, int pageSize);
    Task<ProductDetail?> GetProductAsync(int id);
    Task<CartSummary> GetCartAsync(string sessionKey);
    Task AddToCartAsync(string sessionKey, int productId, int quantity);
    Task UpdateCartAsync(string sessionKey, int productId, int quantity);
    Task RemoveFromCartAsync(string sessionKey, int productId);
}
