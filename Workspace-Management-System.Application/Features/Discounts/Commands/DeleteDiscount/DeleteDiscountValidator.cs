using FluentValidation;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.DeleteDiscount
{
    public class DeleteDiscountValidator
        : AbstractValidator<DeleteDiscountCommand>
    {
        public DeleteDiscountValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Discount ID must be greater than zero.");
        }
    }
}