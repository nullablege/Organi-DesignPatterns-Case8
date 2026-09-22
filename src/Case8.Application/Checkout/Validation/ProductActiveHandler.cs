namespace Case8.Application.Checkout.Validation;

public class ProductActiveHandler : CheckoutValidationHandler
{
    public override Task<CheckoutValidationResult> HandleAsync(CheckoutValidationContext context)
    {
        var inactiveLine = context.Items.FirstOrDefault(x => !x.IsActive);
        if (inactiveLine is not null)
        {
            return Task.FromResult(new CheckoutValidationResult
            {
                IsValid = false,
                ErrorMessage = $"{inactiveLine.Name} is no longer available."
            });
        }

        return base.HandleAsync(context);
    }
}
