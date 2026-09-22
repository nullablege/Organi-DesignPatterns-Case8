namespace Case8.Domain.Orders.States;

public class PreparingOrderState : IOrderState
{
    public string Name => "Preparing";
    public IOrderState Prepare() => throw new InvalidOperationException("Order is already being prepared.");
    public IOrderState Ship() => new ShippedOrderState();
    public IOrderState Deliver() => throw new InvalidOperationException("Order must be shipped before delivery.");
    public IOrderState Cancel() => new CancelledOrderState();
}
