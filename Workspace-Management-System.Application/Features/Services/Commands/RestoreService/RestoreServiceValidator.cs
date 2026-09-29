using FluentValidation;

namespace Workspace_Management_System.Application.Features.Services.Commands.RestoreService
{
    public class RestoreServiceValidator : AbstractValidator<RestoreServiceCommand>
    {
        public RestoreServiceValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Service ID must be greater than 0.");
        }
    }
}
