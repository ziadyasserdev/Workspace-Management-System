using FluentValidation;

namespace Workspace_Management_System.Application.Features.Packages.Commands.UpdatePackage;

public class UpdatePackageValidator
    : AbstractValidator<UpdatePackageCommand>
{
    public UpdatePackageValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Package ID must be greater than zero.");

        RuleFor(x => x.NameEn)
            .NotEmpty()
            .WithMessage("English package name is required.")
            .MaximumLength(100)
            .WithMessage("English package name cannot exceed 100 characters.");

        RuleFor(x => x.NameAr)
            .NotEmpty()
            .WithMessage("Arabic package name is required.")
            .MaximumLength(100)
            .WithMessage("Arabic package name cannot exceed 100 characters.");

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(500)
            .WithMessage("English package description cannot exceed 500 characters.");

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(500)
            .WithMessage("Arabic package description cannot exceed 500 characters.");

        RuleFor(x => x.TotalHours)
            .GreaterThan(0)
            .WithMessage("Total hours must be greater than zero.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price cannot be negative.");

        RuleFor(x => x.DurationDays)
            .GreaterThan(0)
            .When(x => x.DurationDays.HasValue)
            .WithMessage("Duration days must be greater than zero.");
    }
}