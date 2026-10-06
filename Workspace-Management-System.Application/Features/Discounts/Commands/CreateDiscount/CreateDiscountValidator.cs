using FluentValidation;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.CreateDiscount
{
    public class CreateDiscountValidator
        : AbstractValidator<CreateDiscountCommand>
    {
        public CreateDiscountValidator()
        {
            RuleFor(x => x.NameEn)
                .NotEmpty()
                .WithMessage("Discount English name is required.")
                .MaximumLength(100)
                .WithMessage("Discount English name must not exceed 100 characters.");

            RuleFor(x => x.NameAr)
                .NotEmpty()
                .WithMessage("Discount Arabic name is required.")
                .MaximumLength(100)
                .WithMessage("Discount Arabic name must not exceed 100 characters.");

            RuleFor(x => x.DescriptionEn)
                .MaximumLength(500)
                .WithMessage("English description must not exceed 500 characters.");

            RuleFor(x => x.DescriptionAr)
                .MaximumLength(500)
                .WithMessage("Arabic description must not exceed 500 characters.");

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