namespace Case8.Application.Shipping;

public class ExpressShippingStrategy : IShippingStrategy
{
    public string Method => "express";
    public decimal Calculate(decimal cartSubtotal) => 99.90m;
}
