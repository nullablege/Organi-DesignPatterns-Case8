using Case8.Application.Checkout;
using Case8.Application.Order.Observers;
using Case8.Domain;
using Microsoft.EntityFrameworkCore;

namespace Case8.Infrastructure;

public class CheckoutService(StoreDbContext db, IUnitOfWork unitOfWork, OrderPlacedPublisher publisher) : ICheckoutService
{
    public async Task<int> PlaceOrderAsync(string sessionKey, string shippingMethod, decimal shippingCost)
    {
        var orderId = 0;
        await unitOfWork.ExecuteAsync(async () =>
        {
            var cart = await db.Carts.Include(x => x.Items).ThenInclude(x => x.Product).SingleOrDefaultAsync(x => x.SessionKey == sessionKey);
            if (cart is null || cart.Items.Count == 0) throw new InvalidOperationException("Your cart is empty.");

            var unavailableLine = cart.Items.FirstOrDefault(x => !x.Product.IsActive || x.Quantity > x.Product.Stock);
            if (unavailableLine is not null) throw new InvalidOperationException($"{unavailableLine.Product.Name} is no longer available in the requested quantity.");

            var subtotal = cart.Items.Sum(x => x.Product.Price * x.Quantity);
            var order = new Order
            {
                CreatedAt = DateTime.UtcNow,
                Status = "Pending",
                Total = subtotal + shippingCost,
                ShippingMethod = shippingMethod,
                ShippingCost = shippingCost
            };

            foreach (var line in cart.Items)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = line.ProductId,
                    ProductName = line.Product.Name,
                    UnitPrice = line.Product.Price,
                    Quantity = line.Quantity
                });
                line.Product.Stock -= line.Quantity;
                db.StockMovements.Add(new StockMovement
                {
                    ProductId = line.ProductId,
                    QuantityChange = -line.Quantity,
                    Reason = "Order checkout",
                    CreatedAt = DateTime.UtcNow
                });
            }

            db.Orders.Add(order);
            db.CartItems.RemoveRange(cart.Items);
            await db.SaveChangesAsync();
            orderId = order.Id;
        });
        await publisher.PublishAsync(orderId);
        return orderId;
    }
}
