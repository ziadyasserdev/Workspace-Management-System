using FluentValidation;

namespace Workspace_Management_System.Application.Features.Services.Commands.CreateService
{
    public class CreateServiceValidator
        : AbstractValidator<CreateServiceCommand>
    {
        public CreateServiceValidator()
        {
            RuleFor(x => x.NameEn)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage(
                    "English service name is required and cannot exceed 100 characters.");

            RuleFor(x => x.NameAr)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage(
                    "Arabic service name is required and cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage(
                    "Service price must be greater than zero.");
        }
    }
}