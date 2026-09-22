namespace Case8.Domain.Orders.States;

public class DeliveredOrderState : IOrderState
{
    public string Name => "Delivered";
    public IOrderState Prepare() => throw new InvalidOperationException("A delivered order cannot change state.");
    public IOrderState Ship() => throw new InvalidOperationException("A delivered order cannot change state.");
    public IOrderState Deliver() => throw new InvalidOperationException("Order is already delivered.");
    public IOrderState Cancel() => throw new InvalidOperationException("A delivered order cannot be cancelled.");
}
