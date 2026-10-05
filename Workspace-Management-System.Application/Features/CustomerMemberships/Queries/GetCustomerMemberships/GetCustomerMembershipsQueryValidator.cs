using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Queries.GetCustomerMemberships
{

    public class GetCustomerMembershipsQueryValidator
    : AbstractValidator<GetCustomerMembershipsQuery>
    {
        public GetCustomerMembershipsQueryValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("Customer ID must be greater than 0.");
        }
    }
}
