using FluentValidation;

namespace Workspace_Management_System.Application.Features.SessionProducts.Queries.GetSessionProducts
{
    public class GetSessionProductsValidator
        : AbstractValidator<GetSessionProductsQuery>
    {
        public GetSessionProductsValidator()
        {
            RuleFor(x => x.SessionId)
                .GreaterThan(0)
                .WithMessage("Session ID must be greater than zero.");
        }
    }
}