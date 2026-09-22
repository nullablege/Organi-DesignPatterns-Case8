namespace Case8.Application.Checkout.Validation;

public class CartNotEmptyHandler : CheckoutValidationHandler
{
    public override Task<CheckoutValidationResult> HandleAsync(CheckoutValidationContext context)
    {
        if (!context.Items.Any())
        {
            return Task.FromResult(new CheckoutValidationResult
            {
                IsValid = false,
                ErrorMessage = "Your cart is empty."
            });
        }

        return base.HandleAsync(context);
    }
}
