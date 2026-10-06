using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.SessionProducts.Queries.GetSessionProducts
{
    public class GetSessionProductsValidator
        : AbstractValidator<GetSessionProductsQuery>
    {
        public GetSessionProductsValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.SessionId)
                .GreaterThan(0)
                .WithMessage(localizer["SessionIdGreaterThanZero"]);
        }
    }
}
