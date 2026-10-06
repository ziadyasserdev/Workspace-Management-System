using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.ProductCategories.Queries.GetProductCategories;

public class GetProductCategoriesQueryValidator
    : AbstractValidator<GetProductCategoriesQuery>
{
    public GetProductCategoriesQueryValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Search)
            .MaximumLength(100)
            .WithMessage(localizer["SearchMaxLength"]);

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage(localizer["PageNumberGreaterThanZero"]);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage(localizer["PageSizeBetweenOneAndOneHundred"]);
    }
}