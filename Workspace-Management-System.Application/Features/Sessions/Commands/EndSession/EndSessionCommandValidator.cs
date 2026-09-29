using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Sessions.Commands.EndSession
{
    public class EndSessionCommandValidator
    : AbstractValidator<EndSessionCommand>
    {
        public EndSessionCommandValidator()
        {
            RuleFor(x => x.SessionId)
                .GreaterThan(0)
                .WithMessage("Session id must be greater than 0.");
        }
    }
}
