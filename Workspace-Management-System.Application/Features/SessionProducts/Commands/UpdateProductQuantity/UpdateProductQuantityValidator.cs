using FluentValidation;

namespace Workspace_Management_System.Application.Features.SessionProducts.Commands.UpdateProductQuantity
{
    public class UpdateProductQuantityValidator
        : AbstractValidator<UpdateProductQuantityCommand>
    {
        public UpdateProductQuantityValidator()
        {
            RuleFor(x => x.SessionId)
                .GreaterThan(0)
                .WithMessage("Session ID must be greater than zero.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Product ID must be greater than zero.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");
        }
    }
}