using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.MembershipBenefits.Dtos;
using Workspace_Management_System.Application.Features.Memberships.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Queries.GetMembershipBenefits
{
    public class GetMembershipBenefitsQueryHandler
        : IRequestHandler<GetMembershipBenefitsQuery, Result<List<MembershipBenefitListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public GetMembershipBenefitsQueryHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<List<MembershipBenefitListDto>>> Handle(
            GetMembershipBenefitsQuery request,
            CancellationToken cancellationToken)
        {
            var membershipExists = await _unitOfWork.Memberships
                .Query()
                .AnyAsync(
                    x => x.Id == request.MembershipId && !x.IsDeleted,
                    cancellationToken);

            if (!membershipExists)
            {
                return Result<List<MembershipBenefitListDto>>.Failure(
                    ResultStatus.NotFound,
                    _localizer["MembershipNotFound"].Value);
            }

            var isArabic =
                CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

            var benefits = await _unitOfWork.MembershipBenefits
                .Query()
                .AsNoTracking()
                .Where(x =>
                    x.MembershipId == request.MembershipId &&
                    !x.IsDeleted)
                .OrderBy(x => x.Id)
                .Select(x => new MembershipBenefitListDto
                {
                    Id = x.Id,
                    BenefitType = x.BenefitType,
                    WorkspaceTypeId = x.WorkspaceTypeId,

                    WorkspaceTypeName = x.WorkspaceType == null
                        ? null
                        : isArabic
                            ? x.WorkspaceType.NameAr
                            : x.WorkspaceType.NameEn,

                    Hours = x.Hours,
                    DiscountPercentage = x.DiscountPercentage,

                    Description = isArabic
                        ? x.DescriptionAr
                        : x.DescriptionEn
                })
                .ToListAsync(cancellationToken);

            return Result<List<MembershipBenefitListDto>>.Success(benefits);
        }
    }
}
