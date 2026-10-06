using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Checkout.Commands.CheckoutSession;

public class CheckoutSessionValidator
    : AbstractValidator<CheckoutSessionCommand>
{
    public CheckoutSessionValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.SessionId)
            .GreaterThan(0)
            .WithMessage(localizer["SessionIdGreaterThanZero"]);

        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage(localizer["CheckoutRequestRequired"]);

        When(x => x.Request != null, () =>
        {
            RuleFor(x => x.Request.DiscountId)
                .GreaterThan(0)
                .When(x => x.Request.DiscountId.HasValue)
                .WithMessage(localizer["DiscountIdGreaterThanZero"]);

            RuleFor(x => x.Request.TaxRate)
                .InclusiveBetween(0, 100)
                .When(x => x.Request.TaxRate.HasValue)
                .WithMessage(localizer["TaxRateMustBeBetween0And100"]);
        });
    }
}
