using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case8.Application.Checkout.Validation
{
    public class CheckoutValidationPipeline
    {
        public CheckoutValidationHandler Create()
        {
            var cartNotEmptyHandler = new CartNotEmptyHandler();
            var productActiveHandler = new ProductActiveHandler();
            var stockAvailableHandler = new StockAvailableHandler();
            var quantityLimitHandler = new QuantityLimitHandler();

            cartNotEmptyHandler.SetNext(productActiveHandler).SetNext(stockAvailableHandler).SetNext(quantityLimitHandler);
            return cartNotEmptyHandler;
        }
    }
}
