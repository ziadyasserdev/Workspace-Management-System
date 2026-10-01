using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Commands.UpdateMembershipBenefit
{
    public class UpdateMembershipBenefitCommandHandler
    : IRequestHandler<
        UpdateMembershipBenefitCommand,
        Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateMembershipBenefitCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(
            UpdateMembershipBenefitCommand request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return Result<bool>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

          
            var membership = await _unitOfWork.Memberships
                .Query()
                .FirstOrDefaultAsync(
                    x =>
                        x.Id == request.MembershipId &&
                        !x.IsDeleted,
                    cancellationToken);

            if (membership == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Membership not found.");
            }

          
            if (!membership.IsActive)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Cannot update benefits of an inactive membership.");
            }

        
            var benefit = await _unitOfWork.MembershipBenefits
                .Query()
                .FirstOrDefaultAsync(
                    x =>
                        x.Id == request.BenefitId &&
                        x.MembershipId == request.MembershipId &&
                        !x.IsDeleted,
                    cancellationToken);

            if (benefit == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Membership benefit not found.");
            }

          
            if (request.WorkspaceTypeId.HasValue)
            {
                var workspaceType = await _unitOfWork.WorkspaceTypes
                    .Query()
                    .FirstOrDefaultAsync(
                        x =>
                            x.Id == request.WorkspaceTypeId.Value &&
                            !x.IsDeleted,
                        cancellationToken);

                if (workspaceType == null)
                {
                    return Result<bool>.Failure(
                        ResultStatus.NotFound,
                        "Workspace type not found.");
                }
            }

        
            benefit.BenefitType = request.BenefitType;

            benefit.WorkspaceTypeId =
                request.WorkspaceTypeId;

            benefit.Hours =
                request.Hours;

            benefit.DiscountPercentage =
                request.DiscountPercentage;

            benefit.Description =
                string.IsNullOrWhiteSpace(request.Description)
                    ? null
                    : request.Description.Trim();

            benefit.UpdatedAt = DateTime.UtcNow;
            benefit.UpdatedBy = currentUserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Membership benefit updated successfully.");
        }
    }


}
