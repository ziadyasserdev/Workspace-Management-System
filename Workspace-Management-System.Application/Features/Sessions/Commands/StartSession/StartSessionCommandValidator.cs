using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Sessions.Commands.StartSession
{
    public class StartSessionCommandValidator
     : AbstractValidator<StartSessionCommand>
    {
        public StartSessionCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["CustomerIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.WorkspaceId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["WorkspaceIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.PricingPlanId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["PricingPlanIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.NumberOfPeople)
                .GreaterThan(0)
                .WithMessage(
                    localizer["NumberOfPeopleMustBeGreaterThanZero"].Value);

            RuleFor(x => x.BookingId)
                .GreaterThan(0)
                .When(x => x.BookingId.HasValue)
                .WithMessage(
                    localizer["BookingIdMustBeGreaterThanZero"].Value);
        }
    }
}
