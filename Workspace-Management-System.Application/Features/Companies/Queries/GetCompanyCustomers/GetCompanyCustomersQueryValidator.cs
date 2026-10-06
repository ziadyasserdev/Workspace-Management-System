using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Companies.Queries.GetCompanyCustomers
{
    public class GetCompanyCustomersQueryValidator
        : AbstractValidator<GetCompanyCustomersQuery>
    {
        public GetCompanyCustomersQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0)
                .WithMessage(localizer["CompanyIdGreaterThanZero"]);

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage(localizer["PageNumberGreaterThanZero"]);

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage(localizer["PageSizeMustBeBetween1And100"]);
        }
    }
}
