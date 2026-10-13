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
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Memberships.Commands.DeleteMembership
{
    public class DeleteMembershipCommandHandler
          : IRequestHandler<DeleteMembershipCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public DeleteMembershipCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
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

            var hasActiveCustomerMembership =
                await _unitOfWork.CustomerMemberships
                    .Query()
                    .AnyAsync(
                        x =>
                            !x.IsDeleted &&
                            x.MembershipId == membership.Id &&
                            x.Status == CustomerMembershipStatus.Active,
                        cancellationToken);

            if (hasActiveCustomerMembership)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["MembershipHasActiveCustomerSubscriptions"].Value);
            }

            var now = DateTime.UtcNow;

            membership.IsDeleted = true;
            membership.IsDeletedBy = currentUserId;
            membership.UpdatedAt = now;
            membership.UpdatedBy = currentUserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["MembershipDeletedSuccessfully"].Value);
        }
    }
}
