
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.SessionProducts.Commands.AddProduct
{
    public class AddProductValidator
        : AbstractValidator<AddProductCommand>
    {
        public AddProductValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.SessionId)
                .GreaterThan(0)
                .WithMessage(localizer["SessionIdGreaterThanZero"]);

            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage(localizer["ProductIdGreaterThanZero"]);

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage(localizer["QuantityMustBeGreaterThanZero"]);
        }
    }
}
