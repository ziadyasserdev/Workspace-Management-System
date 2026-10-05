using FluentValidation;

namespace Workspace_Management_System.Application.Features.Packages.Commands.UpgradeCustomerPackage;

public class UpgradeCustomerPackageValidator
    : AbstractValidator<UpgradeCustomerPackageCommand>
{
    public UpgradeCustomerPackageValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer ID must be greater than zero.");

        RuleFor(x => x.NewPackageId)
            .GreaterThan(0)
            .WithMessage("New package ID must be greater than zero.");
    }
}