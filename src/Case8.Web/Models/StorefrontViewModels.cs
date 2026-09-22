using Case8.Application;

namespace Case8.Web.Models;

public record HomeViewModel(IReadOnlyList<CategoryItem> Categories, IReadOnlyList<ProductListItem> Products);
public record ShopViewModel(ShopResult Result, string? Search, int? CategoryId, string? Sort);
public record CheckoutViewModel(CartSummary Cart, IReadOnlyList<string> Methods, string SelectedMethod, decimal ShippingCost, string? ValidationError);
