using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.DeleteDiscount;

public class DeleteDiscountValidator
    : AbstractValidator<DeleteDiscountCommand>
{
    public DeleteDiscountValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localizer["DiscountIdGreaterThanZero"]);
    }
}
