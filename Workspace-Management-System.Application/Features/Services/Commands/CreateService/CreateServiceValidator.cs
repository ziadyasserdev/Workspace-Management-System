using FluentValidation;

namespace Workspace_Management_System.Application.Features.Services.Commands.CreateService
{
    public class CreateServiceValidator
        : AbstractValidator<CreateServiceCommand>
    {
        public CreateServiceValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage(
                    "Service name is required and cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage(
                    "Service price must be greater than zero.");
        }
    }
}