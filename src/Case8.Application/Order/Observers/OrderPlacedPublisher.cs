namespace Case8.Application.Order.Observers;

public class OrderPlacedPublisher(IEnumerable<IOrderObserver> observers)
{
    public async Task PublishAsync(int orderId)
    {
        foreach (var observer in observers)
        {
            await observer.OnOrderPlacedAsync(orderId);
        }
    }
}
