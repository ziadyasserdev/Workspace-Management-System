
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Customers.Commands.RestoreCustomer
{
    public class RestoreCustomerCommandValidator
        : AbstractValidator<RestoreCustomerCommand>
    {
        public RestoreCustomerCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["CustomerIdGreaterThanZero"]);
        }
    }
}
