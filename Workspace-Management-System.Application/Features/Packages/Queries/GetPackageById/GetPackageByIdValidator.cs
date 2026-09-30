using FluentValidation;

namespace Workspace_Management_System.Application.Features.Packages.Queries.GetPackageById;

public class GetPackageByIdValidator
    : AbstractValidator<GetPackageByIdQuery>
{
    public GetPackageByIdValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Package ID must be greater than zero.");
    }
}