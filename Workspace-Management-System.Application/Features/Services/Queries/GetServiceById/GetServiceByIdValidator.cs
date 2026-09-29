using FluentValidation;

namespace Workspace_Management_System.Application.Features.Services.Queries.GetServiceById
{
    public class GetServiceByIdValidator : AbstractValidator<GetServiceByIdQuery>
    {
        public GetServiceByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Service ID must be greater than 0.");
        }
    }
}
