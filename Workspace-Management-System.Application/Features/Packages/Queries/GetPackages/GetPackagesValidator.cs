using FluentValidation;

namespace Workspace_Management_System.Application.Features.Packages.Queries.GetPackages;

public class GetPackagesValidator
    : AbstractValidator<GetPackagesQuery>
{
    public GetPackagesValidator()
    {
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