using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.SessionProducts.Queries.GetSessionProductById;

public class GetSessionProductByIdQueryValidator
    : AbstractValidator<GetSessionProductByIdQuery>
{
    public GetSessionProductByIdQueryValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localizer["IdGreaterThanZero"]);
    }
}
