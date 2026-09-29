using FluentValidation;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.CreateDiscount
{
    public class CreateDiscountValidator
        : AbstractValidator<CreateDiscountCommand>
    {
        public CreateDiscountValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Value)
                .GreaterThan(0);

            RuleFor(x => x)
                .Must(x =>
                    x.Type != Domain.Enums.DiscountType.Percentage ||
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