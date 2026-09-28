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

        RuleFor(x => x.Request.DiscountAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Discount cannot be negative.");

        RuleFor(x => x.Request.TaxRate)
            .InclusiveBetween(0, 100)
            .WithMessage("Tax rate must be between 0 and 100.");
    }
}