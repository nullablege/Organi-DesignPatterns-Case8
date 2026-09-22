using Case8.Application.Order.Observers;
using Case8.Domain;

namespace Case8.Infrastructure.Orders.Observers;

public class AuditLogObserver(StoreDbContext db) : IOrderObserver
{
    public async Task OnOrderPlacedAsync(int orderId)
    {
        db.AuditLogs.Add(new AuditLog
        {
            Action = $"Order #{orderId} was placed.",
            CreatedAt = DateTime.UtcNow
        });
        await db.SaveChangesAsync();
    }
}
