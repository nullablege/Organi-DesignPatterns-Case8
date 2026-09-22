namespace Case8.Domain.Orders.States;

public class ShippedOrderState : IOrderState
{
    public string Name => "Shipped";
    public IOrderState Prepare() => throw new InvalidOperationException("A shipped order cannot return to preparation.");
    public IOrderState Ship() => throw new InvalidOperationException("Order is already shipped.");
    public IOrderState Deliver() => new DeliveredOrderState();
    public IOrderState Cancel() => throw new InvalidOperationException("A shipped order cannot be cancelled.");
}
