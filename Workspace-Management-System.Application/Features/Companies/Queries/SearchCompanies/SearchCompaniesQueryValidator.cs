using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Companies.Queries.SearchCompanies
{
    public class SearchCompaniesQueryValidator
        : AbstractValidator<SearchCompaniesQuery>
    {
        public SearchCompaniesQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.SearchTerm)
                .NotEmpty()
                .WithMessage(localizer["SearchTermRequired"])
                .MaximumLength(100)
                .WithMessage(localizer["SearchCannotExceed100Characters"]);

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage(localizer["PageNumberGreaterThanZero"]);

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage(localizer["PageSizeMustBeBetween1And100"])
                .LessThanOrEqualTo(100)
                .WithMessage(localizer["PageSizeMustBeBetween1And100"]);
        }
    }
}
