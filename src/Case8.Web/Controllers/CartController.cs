using Case8.Application;
using Microsoft.AspNetCore.Mvc;

namespace Case8.Web.Controllers;

public class CartController(IStorefrontService store) : Controller
{
    public async Task<IActionResult> Index() => View(await store.GetCartAsync(SessionKey));
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int quantity = 1, string? returnUrl = null)
    {
        await store.AddToCartAsync(SessionKey, productId, quantity);
        return LocalRedirect(string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl) ? "/Cart" : returnUrl);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int productId, int quantity)
    {
        await store.UpdateCartAsync(SessionKey, productId, quantity);
        return RedirectToAction("Index");
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int productId)
    {
        await store.RemoveFromCartAsync(SessionKey, productId);
        return RedirectToAction("Index");
    }
    private string SessionKey
    {
        get
        {
            const string key = "cart-key";
            var value = HttpContext.Session.GetString(key);
            if (value is not null) return value;
            value = Guid.NewGuid().ToString("N");
            HttpContext.Session.SetString(key, value);
            return value;
        }
    }
}
