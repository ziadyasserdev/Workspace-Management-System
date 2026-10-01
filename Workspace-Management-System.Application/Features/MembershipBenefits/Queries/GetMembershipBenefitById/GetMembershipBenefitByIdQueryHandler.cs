using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Memberships.Dtos;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Queries.GetMembershipBenefitById
{
    public class GetMembershipBenefitByIdQueryHandler
    : IRequestHandler<
        GetMembershipBenefitByIdQuery,
        Result<MembershipBenefitDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMembershipBenefitByIdQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<MembershipBenefitDto>> Handle(
            GetMembershipBenefitByIdQuery request,
            CancellationToken cancellationToken)
        {
          
            var membershipExists = await _unitOfWork.Memberships
                .Query()
                .AnyAsync(
                    x =>
                        x.Id == request.MembershipId &&
                        !x.IsDeleted,
                    cancellationToken);

            if (!membershipExists)
            {
                return Result<MembershipBenefitDto>.Failure(
                    ResultStatus.NotFound,
                    "Membership not found.");
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

                    WorkspaceTypeName =
                        x.WorkspaceType != null
                            ? x.WorkspaceType.Name
                            : null,

                    Hours = x.Hours,

                    DiscountPercentage =
                        x.DiscountPercentage,

                    Description = x.Description
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (benefit == null)
            {
                return Result<MembershipBenefitDto>.Failure(
                    ResultStatus.NotFound,
                    "Membership benefit not found.");
            }

            return Result<MembershipBenefitDto>.Success(
                benefit);
        }
    }
}
