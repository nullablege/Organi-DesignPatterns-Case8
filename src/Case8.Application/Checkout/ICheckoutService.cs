namespace Case8.Application.Checkout;

public interface ICheckoutService
{
    Task<int> PlaceOrderAsync(string sessionKey, string shippingMethod, decimal shippingCost);
}
