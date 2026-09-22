using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case8.Domain.Orders.States
{
    public interface IOrderState
    {
        public string Name { get; }
        IOrderState Prepare();
        IOrderState Ship();
        IOrderState Deliver();
        IOrderState Cancel();
    }
}
