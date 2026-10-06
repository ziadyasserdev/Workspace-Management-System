using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryValidator
        : AbstractValidator<GetCustomerByIdQuery>
    {
        public GetCustomerByIdQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["CustomerIdGreaterThanZero"]);
        }
    }
}
