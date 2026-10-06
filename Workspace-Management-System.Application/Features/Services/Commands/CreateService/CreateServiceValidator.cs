using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Services.Commands.CreateService
{
    public class CreateServiceValidator
        : AbstractValidator<CreateServiceCommand>
    {
        public CreateServiceValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.NameEn)
                .NotEmpty()
                .WithMessage(localizer["EnglishServiceNameRequired"])
                .MaximumLength(100)
                .WithMessage(localizer["EnglishServiceNameMaxLength"]);

            RuleFor(x => x.NameAr)
                .NotEmpty()
                .WithMessage(localizer["ArabicServiceNameRequired"])
                .MaximumLength(100)
                .WithMessage(localizer["ArabicServiceNameMaxLength"]);

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage(localizer["ServicePriceMustBeGreaterThanZero"]);
        }
    }
}
