namespace Case8.Application.Shipping;

public class ShippingCalculator(IEnumerable<IShippingStrategy> strategies)
{
    public IReadOnlyList<string> Methods { get; } = strategies.Select(x => x.Method).ToList();

    public decimal Calculate(string method, decimal cartSubtotal)
    {
        var strategy = strategies.SingleOrDefault(x => x.Method == method);
        if (strategy is null) throw new ArgumentException("Unknown shipping method.", nameof(method));
        return strategy.Calculate(cartSubtotal);
    }
}
