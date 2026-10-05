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

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Commands.DeleteMembershipBenefit
{
    public class DeleteMembershipBenefitCommandHandler
        : IRequestHandler<
            DeleteMembershipBenefitCommand,
            Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteMembershipBenefitCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(
            DeleteMembershipBenefitCommand request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return Result<bool>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            // 1. Check Membership
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

            // 2. Check Benefit belongs to Membership
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

           
            var now = DateTime.UtcNow;

            benefit.IsDeleted = true;
            benefit.IsDeletedBy = currentUserId;

            benefit.UpdatedAt = now;
            benefit.UpdatedBy = currentUserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Membership benefit deleted successfully.");
        }
    }
}
