using FluentValidation;

namespace Workspace_Management_System.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryValidator
        : AbstractValidator<GetCustomerByIdQuery>
    {
        public GetCustomerByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Customer ID must be greater than 0");
        }
    }
}