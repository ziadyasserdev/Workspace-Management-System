using FluentValidation;

namespace Workspace_Management_System.Application.Features.Customers.Commands.RestoreCustomer
{
    public class RestoreCustomerCommandValidator
        : AbstractValidator<RestoreCustomerCommand>
    {
        public RestoreCustomerCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Customer ID must be greater than 0.");
        }
    }
}