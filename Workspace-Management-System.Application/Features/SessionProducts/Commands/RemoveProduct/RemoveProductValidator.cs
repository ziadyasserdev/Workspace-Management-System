using FluentValidation;

namespace Workspace_Management_System.Application.Features.SessionProducts.Commands.RemoveProduct
{
    public class RemoveProductValidator
        : AbstractValidator<RemoveProductCommand>
    {
        public RemoveProductValidator()
        {
            RuleFor(x => x.SessionId)
                .GreaterThan(0)
                .WithMessage("Session ID must be greater than zero.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Product ID must be greater than zero.");
        }
    }
}