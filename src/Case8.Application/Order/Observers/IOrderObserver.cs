using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case8.Application.Order.Observers
{
    public interface IOrderObserver
    {
        Task OnOrderPlacedAsync(int orderId);
    }
}
