using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case8.Domain.Orders.States
{
    public class PendingOrderState : IOrderState
    {
        public string Name => "Pending";

        public IOrderState Cancel()
        {
            return new CancelledOrderState();
        }

        public IOrderState Deliver()
        {
            throw new InvalidOperationException();
        }

        public IOrderState Prepare()
        {
            return new PreparingOrderState();
        }

        public IOrderState Ship()
        {
            throw new InvalidOperationException();
        }
    }
}
