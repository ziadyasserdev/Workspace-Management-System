using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.MembershipBenefits.Dtos;
using Workspace_Management_System.Application.Features.Memberships.Dtos;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Queries.GetMembershipBenefits
{
    public class GetMembershipBenefitsQueryHandler
     : IRequestHandler<
         GetMembershipBenefitsQuery,
         Result<List<MembershipBenefitDtoo>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMembershipBenefitsQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<MembershipBenefitDtoo>>> Handle(
            GetMembershipBenefitsQuery request,
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
                return Result<List<MembershipBenefitDtoo>>.Failure(
                    ResultStatus.NotFound,
                    "Membership not found.");
            }

            var benefits = await _unitOfWork.MembershipBenefits
                .Query()
                .AsNoTracking()
                .Where(x =>
                    x.MembershipId == request.MembershipId &&
                    !x.IsDeleted)
                .OrderBy(x => x.Id)
                .Select(x => new MembershipBenefitDtoo
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
                .ToListAsync(cancellationToken);

            return Result<List<MembershipBenefitDtoo>>
                .Success(benefits);
        }
    }
}
