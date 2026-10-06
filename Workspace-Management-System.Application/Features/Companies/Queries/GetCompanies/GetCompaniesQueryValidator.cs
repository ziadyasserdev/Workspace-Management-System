using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Companies.Queries.GetCompanies
{
    public class GetCompaniesQueryValidator
        : AbstractValidator<GetCompaniesQuery>
    {
        public GetCompaniesQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
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
