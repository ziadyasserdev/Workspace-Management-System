using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Sessions.Commands.StartSession
{
    public class StartSessionCommandValidator
     : AbstractValidator<StartSessionCommand>
    {
        public StartSessionCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("Customer id must be greater than 0.");

            RuleFor(x => x.WorkspaceId)
                .GreaterThan(0)
                .WithMessage("Workspace id must be greater than 0.");

            RuleFor(x => x.PricingPlanId)
                .GreaterThan(0)
                .WithMessage("Pricing plan id must be greater than 0.");

            RuleFor(x => x.NumberOfPeople)
                .GreaterThan(0)
                .WithMessage("Number of people must be greater than 0.");

            RuleFor(x => x.BookingId)
                .GreaterThan(0)
                .When(x => x.BookingId.HasValue)
                .WithMessage("Booking id must be greater than 0.");
        }
    }
}
