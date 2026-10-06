
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Discounts.Queries.GetDiscountById;

public class GetDiscountByIdValidator
    : AbstractValidator<GetDiscountByIdQuery>
{
    public GetDiscountByIdValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localizer["DiscountIdGreaterThanZero"]);
    }
}
