using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Sessions.Queries.GetSessionDetails
{
    public class GetSessionDetailsQueryValidator
     : AbstractValidator<GetSessionDetailsQuery>
    {
        public GetSessionDetailsQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Session id must be greater than 0.");
        }
    }
}
