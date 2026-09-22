using Case8.Application;
using Case8.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Case8.Web.Controllers;

public class ShopController(IStorefrontService store) : Controller
{
    public async Task<IActionResult> Index(string? search, int? categoryId, string? sort, int page = 1)
    {
        var result = await store.GetShopAsync(search, categoryId, sort, Math.Max(page, 1), 6);
        return View(new ShopViewModel(result, search, categoryId, sort));
    }

    public async Task<IActionResult> Detail(int id)
    {
        var product = await store.GetProductAsync(id);
        return product is null ? NotFound() : View(product);
    }
}
