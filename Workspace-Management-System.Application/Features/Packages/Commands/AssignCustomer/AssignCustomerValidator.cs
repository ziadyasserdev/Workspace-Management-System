using FluentValidation;

namespace Workspace_Management_System.Application.Features.Packages.Commands.AssignCustomer;

public class AssignCustomerValidator
    : AbstractValidator<AssignCustomerCommand>
{
    public AssignCustomerValidator()
    {
        RuleFor(x => x.PackageId)
            .GreaterThan(0)
            .WithMessage("Package ID must be greater than zero.");

        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer ID must be greater than zero.");
    }
}