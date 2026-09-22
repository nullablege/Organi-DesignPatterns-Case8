namespace Case8.Domain.Orders.States;

public class CancelledOrderState : IOrderState
{
    public string Name => "Cancelled";
    public IOrderState Prepare() => throw new InvalidOperationException("A cancelled order cannot change state.");
    public IOrderState Ship() => throw new InvalidOperationException("A cancelled order cannot change state.");
    public IOrderState Deliver() => throw new InvalidOperationException("A cancelled order cannot change state.");
    public IOrderState Cancel() => throw new InvalidOperationException("Order is already cancelled.");
}
