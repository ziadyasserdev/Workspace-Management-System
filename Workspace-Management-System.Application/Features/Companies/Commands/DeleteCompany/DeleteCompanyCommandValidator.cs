using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Companies.Commands.DeleteCompany
{
    public class DeleteCompanyCommandValidator
        : AbstractValidator<DeleteCompanyCommand>
    {
        public DeleteCompanyCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["CompanyIdGreaterThanZero"]);
        }
    }
}
