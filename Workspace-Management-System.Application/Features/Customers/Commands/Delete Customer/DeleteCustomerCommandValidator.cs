using FluentValidation;
using Workspace_Management_System.Application.Features.Customers.Commands.Delete_Customer;

namespace Workspace_Management_System.Application.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandValidator
        : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Customer ID must be greater than 0");
        }
    }
}