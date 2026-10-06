
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Services.Queries.GetServiceById
{
    public class GetServiceByIdValidator : AbstractValidator<GetServiceByIdQuery>
    {
        public GetServiceByIdValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["ServiceIdGreaterThanZero"]);
        }
    }
}
