using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Packages.Commands.AssignCustomer;

public class AssignCustomerValidator
    : AbstractValidator<AssignCustomerCommand>
{
    public AssignCustomerValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.PackageId)
            .GreaterThan(0)
            .WithMessage(localizer["PackageIdGreaterThanZero"]);

        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage(localizer["CustomerIdGreaterThanZero"]);
    }
}

