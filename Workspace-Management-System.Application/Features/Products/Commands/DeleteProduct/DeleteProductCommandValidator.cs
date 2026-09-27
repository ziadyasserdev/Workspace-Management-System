using FluentValidation;

namespace Workspace_Management_System.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandValidator
        : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Product ID must be greater than 0.");
        }
    }
}