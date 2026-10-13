using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Memberships.Queries.GetMemberships
{
    public class GetMembershipsQueryValidator
       : AbstractValidator<GetMembershipsQuery>
    {
        public GetMembershipsQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage(
                    localizer["PageNumberMustBeGreaterThanZero"].Value);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage(
                    localizer["PageSizeMustBeBetween1And100"].Value);

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Search))
                .WithMessage(
                    localizer["SearchMaxLength"].Value);
        }
    }
}
