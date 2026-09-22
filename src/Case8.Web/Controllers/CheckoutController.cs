using Case8.Application;
using Case8.Application.Shipping;
using Case8.Application.Checkout.Validation;
using Case8.Application.Checkout;
using Case8.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Case8.Web.Controllers;

public class CheckoutController(IStorefrontService store, ShippingCalculator shipping, CheckoutValidationPipeline validationPipeline, ICheckoutService checkout) : Controller
{
    public async Task<IActionResult> Index(string method = "standard")
    {
        var cart = await store.GetCartAsync(SessionKey);
        if (!cart.Items.Any()) return RedirectToAction("Index", "Cart");
        if (!shipping.Methods.Contains(method)) method = "standard";
        var validation = await validationPipeline.Create().HandleAsync(new CheckoutValidationContext(cart.Items));
        var shippingCost = validation.IsValid ? shipping.Calculate(method, cart.Total) : 0m;
        return View(new CheckoutViewModel(cart, shipping.Methods, method, shippingCost, validation.ErrorMessage));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(string method)
    {
        var cart = await store.GetCartAsync(SessionKey);
        if (!shipping.Methods.Contains(method)) return BadRequest();
        var validation = await validationPipeline.Create().HandleAsync(new CheckoutValidationContext(cart.Items));
        if (!validation.IsValid) return RedirectToAction("Index", new { method });

        var orderId = await checkout.PlaceOrderAsync(SessionKey, method, shipping.Calculate(method, cart.Total));
        return RedirectToAction("Success", new { id = orderId });
    }

    public IActionResult Success(int id) => View(id);

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
