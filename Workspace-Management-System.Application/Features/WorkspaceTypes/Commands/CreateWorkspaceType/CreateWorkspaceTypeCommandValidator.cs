using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.CreateWorkspaceType
{
    public class CreateWorkspaceTypeCommandValidator
        : AbstractValidator<CreateWorkspaceTypeCommand>
    {
        public CreateWorkspaceTypeCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.NameEn)
                .NotEmpty()
                .WithMessage(localizer["WorkspaceTypeEnglishNameRequired"])
                .MaximumLength(100)
                .WithMessage(localizer["WorkspaceTypeEnglishNameMaxLength"]);

            RuleFor(x => x.NameAr)
                .NotEmpty()
                .WithMessage(localizer["WorkspaceTypeArabicNameRequired"])
                .MaximumLength(100)
                .WithMessage(localizer["WorkspaceTypeArabicNameMaxLength"]);

            RuleFor(x => x.DescriptionEn)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.DescriptionEn))
                .WithMessage(localizer["WorkspaceTypeEnglishDescriptionMaxLength"]);

            RuleFor(x => x.DescriptionAr)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.DescriptionAr))
                .WithMessage(localizer["WorkspaceTypeArabicDescriptionMaxLength"]);
        }
    }

}
