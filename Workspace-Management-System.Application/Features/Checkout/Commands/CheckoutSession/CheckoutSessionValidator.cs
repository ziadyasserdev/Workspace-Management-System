using FluentValidation;

namespace Workspace_Management_System.Application.Features.Checkout.Commands.CheckoutSession;

public class CheckoutSessionValidator
    : AbstractValidator<CheckoutSessionCommand>
{
    public CheckoutSessionValidator()
    {
        RuleFor(x => x.SessionId)
            .GreaterThan(0)
            .WithMessage("Session ID must be greater than zero.");

        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("Checkout request is required.");

        When(x => x.Request != null, () =>
        {
            RuleFor(x => x.Request.DiscountId)
                .GreaterThan(0)
                .When(x => x.Request.DiscountId.HasValue)
                .WithMessage("Discount ID must be greater than zero.");

            RuleFor(x => x.Request.TaxRate)
                .InclusiveBetween(0, 100)
                .When(x => x.Request.TaxRate.HasValue)
                .WithMessage("Tax rate must be between 0 and 100.");
        });
    }
}