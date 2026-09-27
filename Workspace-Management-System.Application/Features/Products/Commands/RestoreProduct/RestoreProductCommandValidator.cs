using FluentValidation;

namespace Workspace_Management_System.Application.Features.Products.Commands.RestoreProduct
{
    public class RestoreProductCommandValidator
        : AbstractValidator<RestoreProductCommand>
    {
        public RestoreProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Product ID must be greater than 0.");
        }
    }
}