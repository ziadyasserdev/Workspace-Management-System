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

namespace Workspace_Management_System.Application.Features.Memberships.Queries.GetMembershipById
{
    public class GetMembershipByIdQueryHandler
       : IRequestHandler<
           GetMembershipByIdQuery,
           Result<MembershipDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public GetMembershipByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<MembershipDetailsDto>> Handle(
            GetMembershipByIdQuery request,
            CancellationToken cancellationToken)
        {
            var membership = await _unitOfWork.Memberships
                .Query()
                .AsNoTracking()
                .Where(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted)
                .Select(x => new MembershipDetailsDto
                {
                    Id = x.Id,

                    NameEn = x.NameEn,
                    NameAr = x.NameAr,

                    DescriptionEn = x.DescriptionEn,
                    DescriptionAr = x.DescriptionAr,

                    DurationDays = x.DurationDays,
                    Price = x.Price,
                    IsActive = x.IsActive,

                    Benefits = x.Benefits
                        .Where(b => !b.IsDeleted)
                        .Select(b => new MembershipBenefitDto
                        {
                            Id = b.Id,

                            BenefitType = b.BenefitType,

                            WorkspaceTypeId = b.WorkspaceTypeId,

                            WorkspaceTypeNameEn =
                                b.WorkspaceType != null
                                    ? b.WorkspaceType.NameEn
                                    : null,

                            WorkspaceTypeNameAr =
                                b.WorkspaceType != null
                                    ? b.WorkspaceType.NameAr
                                    : null,

                            Hours = b.Hours,

                            DiscountPercentage = b.DiscountPercentage,

                            DescriptionEn = b.DescriptionEn,
                            DescriptionAr = b.DescriptionAr
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (membership == null)
            {
                return Result<MembershipDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    _localizer["MembershipNotFound"].Value);
            }

            return Result<MembershipDetailsDto>.Success(membership);
        }
    }
}
