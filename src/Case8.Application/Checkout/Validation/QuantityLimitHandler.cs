namespace Case8.Application.Checkout.Validation;

public class QuantityLimitHandler : CheckoutValidationHandler
{
    public override Task<CheckoutValidationResult> HandleAsync(CheckoutValidationContext context)
    {
        var overLimitLine = context.Items.FirstOrDefault(x => x.Quantity > 10);
        if (overLimitLine is not null)
        {
            return Task.FromResult(new CheckoutValidationResult
            {
                IsValid = false,
                ErrorMessage = $"You can order at most 10 units of {overLimitLine.Name}."
            });
        }

        return base.HandleAsync(context);
    }
}
