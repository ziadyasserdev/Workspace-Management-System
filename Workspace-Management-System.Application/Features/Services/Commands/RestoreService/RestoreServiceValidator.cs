using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Services.Commands.RestoreService
{
    public class RestoreServiceValidator : AbstractValidator<RestoreServiceCommand>
    {
        public RestoreServiceValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["ServiceIdGreaterThanZero"]);
        }
    }
}
