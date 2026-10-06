using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.DeleteProductCategory;

public class DeleteProductCategoryCommandValidator
    : AbstractValidator<DeleteProductCategoryCommand>
{
    public DeleteProductCategoryCommandValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localizer["ProductCategoryIdGreaterThanZero"]);
    }
}