using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Memberships.Commands.ChangeMembershipStatus
{
    public class ChangeMembershipStatusCommandHandler
        : IRequestHandler<ChangeMembershipStatusCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ChangeMembershipStatusCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
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
                    _localizer["UserNotAuthenticated"].Value);
            }

            var membership = await _unitOfWork.Memberships
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.IsDeleted,
                    cancellationToken);

            if (membership == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["MembershipNotFound"].Value);
            }

            if (membership.IsActive == request.IsActive)
            {
                var messageKey = request.IsActive
                    ? "MembershipAlreadyActive"
                    : "MembershipAlreadyInactive";

                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer[messageKey].Value);
            }

            membership.IsActive = request.IsActive;
            membership.UpdatedAt = DateTime.UtcNow;
            membership.UpdatedBy = currentUserId;

            await _unitOfWork.SaveAsync();

            var successMessageKey = request.IsActive
                ? "MembershipActivatedSuccessfully"
                : "MembershipDeactivatedSuccessfully";

            return Result<bool>.Success(
                true,
                _localizer[successMessageKey].Value);
        }
    }
}
