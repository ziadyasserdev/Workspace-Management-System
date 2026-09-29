using FluentValidation;

namespace Workspace_Management_System.Application.Features.Services.Commands.UpdateService
{
    public class UpdateServiceValidator : AbstractValidator<UpdateServiceCommand>
    {
        public UpdateServiceValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Service ID must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Service name is required.")
                .MaximumLength(100)
                .WithMessage("Service name cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Service price must be greater than 0.");
        }
    }
}
