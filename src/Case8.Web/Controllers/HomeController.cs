using Case8.Application;
using Case8.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Case8.Web.Controllers;

public class HomeController(IStorefrontService store) : Controller
{
    public async Task<IActionResult> Index() => View(new HomeViewModel(await store.GetCategoriesAsync(), await store.GetFeaturedProductsAsync()));
}
