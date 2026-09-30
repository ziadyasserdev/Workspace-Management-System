using FluentValidation;

namespace Workspace_Management_System.Application.Features.Packages.Commands.RemoveCustomer;

public class RemoveCustomerValidator
    : AbstractValidator<RemoveCustomerCommand>
{
    public RemoveCustomerValidator()
    {
        RuleFor(x => x.PackageId)
            .GreaterThan(0)
            .WithMessage("Package ID must be greater than zero.");

        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer ID must be greater than zero.");
    }
}