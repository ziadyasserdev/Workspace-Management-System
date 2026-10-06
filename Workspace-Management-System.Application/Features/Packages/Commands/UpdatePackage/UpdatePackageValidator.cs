using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Packages.Commands.UpdatePackage;

public class UpdatePackageValidator
    : AbstractValidator<UpdatePackageCommand>
{
    public UpdatePackageValidator(IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localizer["PackageIdGreaterThanZero"]);

        RuleFor(x => x.NameEn)
            .NotEmpty()
            .WithMessage(localizer["EnglishPackageNameRequired"])
            .MaximumLength(100)
            .WithMessage(localizer["EnglishPackageNameMaxLength"]);

        RuleFor(x => x.NameAr)
            .NotEmpty()
            .WithMessage(localizer["ArabicPackageNameRequired"])
            .MaximumLength(100)
            .WithMessage(localizer["ArabicPackageNameMaxLength"]);

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(500)
            .WithMessage(localizer["EnglishPackageDescriptionMaxLength"]);

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(500)
            .WithMessage(localizer["ArabicPackageDescriptionMaxLength"]);

        RuleFor(x => x.TotalHours)
            .GreaterThan(0)
            .WithMessage(localizer["TotalHoursMustBeGreaterThanZero"]);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage(localizer["PriceCannotBeNegative"]);

        RuleFor(x => x.DurationDays)
            .GreaterThan(0)
            .When(x => x.DurationDays.HasValue)
            .WithMessage(localizer["DurationDaysMustBeGreaterThanZero"]);
    }
}
