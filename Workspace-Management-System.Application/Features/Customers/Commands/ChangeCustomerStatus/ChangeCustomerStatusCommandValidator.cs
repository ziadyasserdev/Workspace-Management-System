using FluentValidation;

namespace Workspace_Management_System.Application.Features.Customers.Commands.ChangeCustomerStatus
{
    public class ChangeCustomerStatusCommandValidator
        : AbstractValidator<ChangeCustomerStatusCommand>
    {
        public ChangeCustomerStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Customer ID must be greater than 0");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid customer status");
        }
    }
}