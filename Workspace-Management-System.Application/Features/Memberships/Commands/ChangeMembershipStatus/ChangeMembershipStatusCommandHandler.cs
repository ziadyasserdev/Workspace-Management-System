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

namespace Workspace_Management_System.Application.Features.Memberships.Commands.ChangeMembershipStatus
{
    public class ChangeMembershipStatusCommandHandler
    : IRequestHandler<
        ChangeMembershipStatusCommand,
        Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public ChangeMembershipStatusCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(
            ChangeMembershipStatusCommand request,
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
                    x => x.Id == request.Id &&
                         !x.IsDeleted,
                    cancellationToken);

            if (membership == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Membership not found.");
            }

            if (membership.IsActive == request.IsActive)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    request.IsActive
                        ? "Membership is already active."
                        : "Membership is already inactive.");
            }

            membership.IsActive = request.IsActive;
            membership.UpdatedAt = DateTime.UtcNow;
            membership.UpdatedBy = currentUserId;

            await _unitOfWork.SaveAsync();

            var message = request.IsActive
                ? "Membership activated successfully."
                : "Membership deactivated successfully.";

            return Result<bool>.Success(true, message);
        }
    }

}
