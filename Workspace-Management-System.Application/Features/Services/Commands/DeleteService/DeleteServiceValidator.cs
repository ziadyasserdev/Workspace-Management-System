using FluentValidation;

namespace Workspace_Management_System.Application.Features.Services.Commands.DeleteService
{
    public class DeleteServiceValidator : AbstractValidator<DeleteServiceCommand>
    {
        public DeleteServiceValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Service ID must be greater than 0.");
        }
    }
}
