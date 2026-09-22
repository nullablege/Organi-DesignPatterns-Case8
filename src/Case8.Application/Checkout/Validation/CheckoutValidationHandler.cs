using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case8.Application.Checkout.Validation
{
    public abstract class CheckoutValidationHandler
    {
        public CheckoutValidationHandler? next;
        public CheckoutValidationHandler SetNext(CheckoutValidationHandler handler) {
            next = handler;
            return handler;
        }


        public virtual Task<CheckoutValidationResult> HandleAsync(CheckoutValidationContext context)
        {
            if(next is not null)
                return next.HandleAsync(context);

            return Task.FromResult(new CheckoutValidationResult { IsValid = true } );
        }

       }
}
