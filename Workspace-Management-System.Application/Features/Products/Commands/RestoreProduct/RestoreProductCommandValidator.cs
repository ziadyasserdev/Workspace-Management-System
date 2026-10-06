
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Products.Commands.RestoreProduct
{
    public class RestoreProductCommandValidator
        : AbstractValidator<RestoreProductCommand>
    {
        public RestoreProductCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["ProductIdGreaterThanZero"]);
        }
    }
}
