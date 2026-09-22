using Case8.Application.Order.Observers;
using Case8.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case8.Infrastructure.Orders.Observers
{
    public class AdminNotificationObserver : IOrderObserver
    {
        private readonly StoreDbContext _context;
        public AdminNotificationObserver(StoreDbContext context) {
            _context = context;
        }
        public async Task OnOrderPlacedAsync(int orderId )
        {
            var adminNotification = new AdminNotification
            {
                Message = $"Order #{orderId} was placed",
                CreatedAt = DateTime.UtcNow
            };
            await _context.AdminNotifications.AddAsync(adminNotification);
            await _context.SaveChangesAsync();
        }
    }
}
