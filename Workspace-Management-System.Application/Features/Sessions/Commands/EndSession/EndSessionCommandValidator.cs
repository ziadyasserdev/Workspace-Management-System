using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Sessions.Commands.EndSession
{
    public class EndSessionCommandValidator
      : AbstractValidator<EndSessionCommand>
    {
        public EndSessionCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.SessionId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["SessionIdMustBeGreaterThanZero"].Value);
        }
    }
}
