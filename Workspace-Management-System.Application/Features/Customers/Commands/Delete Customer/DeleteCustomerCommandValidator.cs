
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Application.Features.Customers.Commands.Delete_Customer;

namespace Workspace_Management_System.Application.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandValidator
    : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localizer["CustomerIdGreaterThanZero"]);
    }
}