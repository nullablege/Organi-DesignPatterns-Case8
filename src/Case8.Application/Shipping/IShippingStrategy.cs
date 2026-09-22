namespace Case8.Application.Shipping;

public interface IShippingStrategy
{
    string Method { get; }
    decimal Calculate(decimal cartSubtotal);
}
