using FluentValidation;

namespace Workspace_Management_System.Application.Features.Packages.Queries.GetPackageCustomers;

public class GetPackageCustomersValidator
    : AbstractValidator<GetPackageCustomersQuery>
{
    public GetPackageCustomersValidator()
    {
        RuleFor(x => x.PackageId)
            .GreaterThan(0)
            .WithMessage("Package ID must be greater than zero.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than zero.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.Search)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Search))
            .WithMessage("Search cannot exceed 100 characters.");
    }
}