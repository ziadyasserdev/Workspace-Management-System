using FluentValidation;

namespace Workspace_Management_System.Application.Features.SessionProducts.Commands.ClearProducts
{
    public class ClearProductsValidator
        : AbstractValidator<ClearProductsCommand>
    {
        public ClearProductsValidator()
        {
            RuleFor(x => x.SessionId)
                .GreaterThan(0)
                .WithMessage("Session ID must be greater than zero.");
        }
    }
}