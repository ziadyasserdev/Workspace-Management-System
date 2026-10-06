
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryValidator
        : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["ProductIdGreaterThanZero"]);
        }
    }
}
