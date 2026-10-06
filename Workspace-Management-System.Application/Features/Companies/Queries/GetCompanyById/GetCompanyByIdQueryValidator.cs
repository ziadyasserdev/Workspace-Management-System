using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Companies.Queries.GetCompanyById
{
    public class GetCompanyByIdQueryValidator
        : AbstractValidator<GetCompanyByIdQuery>
    {
        public GetCompanyByIdQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["CompanyIdGreaterThanZero"]);
        }
    }
}
