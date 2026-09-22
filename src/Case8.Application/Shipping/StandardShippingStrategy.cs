namespace Case8.Application.Shipping;

public class StandardShippingStrategy : IShippingStrategy
{
    public string Method => "standard";

    public decimal Calculate(decimal cartSubtotal)
    {
        if (cartSubtotal >= 750m) return 0m;
        return 49.90m;
    }
}
