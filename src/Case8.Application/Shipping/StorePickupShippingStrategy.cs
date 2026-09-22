namespace Case8.Application.Shipping;

public class StorePickupShippingStrategy : IShippingStrategy
{
    public string Method => "pickup";
    public decimal Calculate(decimal cartSubtotal) => 0m;
}
