using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Memberships.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Queries.GetMembershipBenefitById
{
    public class GetMembershipBenefitByIdQueryHandler
           : IRequestHandler<GetMembershipBenefitByIdQuery, Result<MembershipBenefitDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public GetMembershipBenefitByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<MembershipBenefitDto>> Handle(
            GetMembershipBenefitByIdQuery request,
            CancellationToken cancellationToken)
        {
            var membershipExists = await _unitOfWork.Memberships
                .Query()
                .AnyAsync(
                    x => x.Id == request.MembershipId && !x.IsDeleted,
                    cancellationToken);

            if (!membershipExists)
            {
                return Result<MembershipBenefitDto>.Failure(
                    ResultStatus.NotFound,
                    _localizer["MembershipNotFound"].Value);
            }

            var benefit = await _unitOfWork.MembershipBenefits
                .Query()
                .AsNoTracking()
                .Where(x =>
                    x.Id == request.BenefitId &&
                    x.MembershipId == request.MembershipId &&
                    !x.IsDeleted)
                .Select(x => new MembershipBenefitDto
                {
                    Id = x.Id,
                    BenefitType = x.BenefitType,
                    WorkspaceTypeId = x.WorkspaceTypeId,

                    WorkspaceTypeNameEn = x.WorkspaceType != null
                        ? x.WorkspaceType.NameEn
                        : null,

                    WorkspaceTypeNameAr = x.WorkspaceType != null
                        ? x.WorkspaceType.NameAr
                        : null,

                    Hours = x.Hours,
                    DiscountPercentage = x.DiscountPercentage,
                    DescriptionEn = x.DescriptionEn,
                    DescriptionAr = x.DescriptionAr
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (benefit == null)
            {
                return Result<MembershipBenefitDto>.Failure(
                    ResultStatus.NotFound,
                    _localizer["MembershipBenefitNotFound"].Value);
            }

            return Result<MembershipBenefitDto>.Success(benefit);
        }
    }
}
