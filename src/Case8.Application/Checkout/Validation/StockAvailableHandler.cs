using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case8.Application.Checkout.Validation
{
    public class StockAvailableHandler:CheckoutValidationHandler
    {
        public override Task<CheckoutValidationResult> HandleAsync(CheckoutValidationContext context)
        {
            var unavailableLine = context.Items.FirstOrDefault(line => line.Quantity > line.Stock);

            if (unavailableLine is not null)
            {
                return Task.FromResult(new CheckoutValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"{unavailableLine.Name} için yeterli stok yok."
                });
            }
            return base.HandleAsync(context);
        }

    }
}
