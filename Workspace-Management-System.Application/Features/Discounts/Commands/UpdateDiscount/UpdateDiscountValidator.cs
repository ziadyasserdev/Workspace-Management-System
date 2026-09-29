using FluentValidation;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.UpdateDiscount
{
    public class UpdateDiscountValidator
        : AbstractValidator<UpdateDiscountCommand>
    {
        public UpdateDiscountValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Value)
                .GreaterThan(0);

            RuleFor(x => x)
                .Must(x =>
                    x.Type != DiscountType.Percentage ||
                    x.Value <= 100)
                .WithMessage(
                    "Percentage discount cannot be greater than 100.");

            RuleFor(x => x)
                .Must(x =>
                    !x.StartDate.HasValue ||
                    !x.EndDate.HasValue ||
                    x.EndDate >= x.StartDate)
                .WithMessage(
                    "End date cannot be before start date.");
        }
    }
}