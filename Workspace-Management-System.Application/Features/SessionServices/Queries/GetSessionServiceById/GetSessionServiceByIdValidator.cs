using FluentValidation;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Queries.GetSessionServiceById;

public class GetSessionServiceByIdValidator
    : AbstractValidator<GetSessionServiceByIdQuery>
{
    public GetSessionServiceByIdValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Session service ID must be greater than zero.");
    }
}