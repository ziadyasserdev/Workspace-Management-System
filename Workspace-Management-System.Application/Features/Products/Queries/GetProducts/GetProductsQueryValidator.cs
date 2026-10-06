
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryValidator
        : AbstractValidator<GetProductsQuery>
    {
        public GetProductsQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Search)
                .MaximumLength(150)
                .WithMessage(localizer["ProductSearchMaxLength"]);

            RuleFor(x => x.ProductCategoryId)
                .GreaterThan(0)
                .When(x => x.ProductCategoryId.HasValue)
                .WithMessage(localizer["ProductCategoryIdGreaterThanZero"]);

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage(localizer["PageNumberGreaterThanZero"]);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage(localizer["PageSizeMustBeBetween1And100"]);
        }
    }
}
