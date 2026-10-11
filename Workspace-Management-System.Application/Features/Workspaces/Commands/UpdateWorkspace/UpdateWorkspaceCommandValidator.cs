using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Workspaces.Commands.UpdateWorkspace
{
    public class UpdateWorkspaceCommandValidator
       : AbstractValidator<UpdateWorkspaceCommand>
    {
        public UpdateWorkspaceCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(
                    localizer["WorkspaceIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.WorkspaceTypeId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["WorkspaceTypeIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.NameEn)
                .NotEmpty()
                .WithMessage(
                    localizer["WorkspaceEnglishNameRequired"].Value)
                .MaximumLength(150)
                .WithMessage(
                    localizer["WorkspaceEnglishNameMaxLength"].Value);

            RuleFor(x => x.NameAr)
                .NotEmpty()
                .WithMessage(
                    localizer["WorkspaceArabicNameRequired"].Value)
                .MaximumLength(150)
                .WithMessage(
                    localizer["WorkspaceArabicNameMaxLength"].Value);

            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(
                    localizer["WorkspaceCodeRequired"].Value)
                .MaximumLength(50)
                .WithMessage(
                    localizer["WorkspaceCodeMaxLength"].Value);

            RuleFor(x => x.Floor)
                .MaximumLength(50)
                .WithMessage(
                    localizer["WorkspaceFloorMaxLength"].Value);

            RuleFor(x => x.Location)
                .MaximumLength(200)
                .WithMessage(
                    localizer["WorkspaceLocationMaxLength"].Value);

            RuleFor(x => x.Capacity)
                .GreaterThan(0)
                .WithMessage(
                    localizer["WorkspaceCapacityMustBeGreaterThanZero"].Value);

            RuleFor(x => x.DescriptionEn)
                .MaximumLength(500)
                .WithMessage(
                    localizer["WorkspaceEnglishDescriptionMaxLength"].Value);

            RuleFor(x => x.DescriptionAr)
                .MaximumLength(500)
                .WithMessage(
                    localizer["WorkspaceArabicDescriptionMaxLength"].Value);
        }
    }

}
