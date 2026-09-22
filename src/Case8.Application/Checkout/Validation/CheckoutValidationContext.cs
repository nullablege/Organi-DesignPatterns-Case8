using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case8.Application.Checkout.Validation
{
    public class CheckoutValidationContext
    {
        public IReadOnlyList<CartLine> Items { get; }

        public CheckoutValidationContext(IReadOnlyList<CartLine> items)
        {
            Items = items;
        }
    }
}
