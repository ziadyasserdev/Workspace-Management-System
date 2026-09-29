using FluentValidation;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.RestoreDiscount
{
    public class RestoreDiscountValidator
        : AbstractValidator<RestoreDiscountCommand>
    {
        public RestoreDiscountValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Discount ID must be greater than zero.");
        }
    }
}