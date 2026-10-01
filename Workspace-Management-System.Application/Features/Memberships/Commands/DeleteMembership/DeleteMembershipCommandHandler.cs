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
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Memberships.Commands.DeleteMembership
{
    public class DeleteMembershipCommandHandler
       : IRequestHandler<
           DeleteMembershipCommand,
           Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteMembershipCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(
            DeleteMembershipCommand request,
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

            var hasActiveCustomerMembership =
                await _unitOfWork.CustomerMemberships
                    .Query()
                    .AnyAsync(
                        x =>
                            !x.IsDeleted &&
                            x.MembershipId == membership.Id &&
                            x.Status ==
                                CustomerMembershipStatus.Active,
                        cancellationToken);

            if (hasActiveCustomerMembership)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Membership cannot be deleted because it has active customer subscriptions.");
            }

            var now = DateTime.UtcNow;

            membership.IsDeleted = true;
            membership.IsDeletedBy = currentUserId;

            membership.UpdatedAt = now;
            membership.UpdatedBy = currentUserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Membership deleted successfully.");
        }
    }
}
