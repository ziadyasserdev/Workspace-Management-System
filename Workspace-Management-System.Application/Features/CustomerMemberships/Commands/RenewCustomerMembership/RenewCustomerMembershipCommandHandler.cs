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
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Commands.RenewCustomerMembership
{
    public class RenewCustomerMembershipCommandHandler
       : IRequestHandler<RenewCustomerMembershipCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public RenewCustomerMembershipCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(
            RenewCustomerMembershipCommand request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    _localizer["UserNotAuthenticated"].Value);
            }

            var oldMembership =
                await _unitOfWork.CustomerMemberships
                    .Query()
                    .FirstOrDefaultAsync(
                        x => x.Id == request.Id && !x.IsDeleted,
                        cancellationToken);

            if (oldMembership == null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    _localizer["CustomerMembershipNotFound"].Value);
            }

            if (oldMembership.Status == CustomerMembershipStatus.Active ||
                oldMembership.Status == CustomerMembershipStatus.Suspended)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    _localizer["ActiveOrSuspendedMembershipCannotBeRenewed"].Value);
            }

            var membership =
                await _unitOfWork.Memberships
                    .Query()
                    .FirstOrDefaultAsync(
                        x => x.Id == oldMembership.MembershipId &&
                             !x.IsDeleted,
                        cancellationToken);

            if (membership == null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    _localizer["MembershipPlanNotFound"].Value);
            }

            if (!membership.IsActive)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    _localizer["MembershipPlanIsInactive"].Value);
            }

            var hasCurrentMembership =
                await _unitOfWork.CustomerMemberships
                    .Query()
                    .AnyAsync(
                        x => x.CustomerId == oldMembership.CustomerId &&
                             !x.IsDeleted &&
                             (x.Status == CustomerMembershipStatus.Active ||
                              x.Status == CustomerMembershipStatus.Suspended),
                        cancellationToken);

            if (hasCurrentMembership)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    _localizer["CustomerAlreadyHasActiveOrSuspendedMembership"].Value);
            }

            var now = DateTime.UtcNow;

            var newMembership = new CustomerMembership
            {
                CustomerId = oldMembership.CustomerId,
                MembershipId = oldMembership.MembershipId,
                StartDate = now,
                EndDate = now.AddDays(membership.DurationDays),
                Status = CustomerMembershipStatus.Active,
                PricePaid = membership.Price,
                CreatedAt = now,
                CreatedBy = currentUserId
            };

            await _unitOfWork.CustomerMemberships
                .AddAsync(newMembership);

            await _unitOfWork.SaveAsync();

            return Result<int>.Success(
                newMembership.Id,
                _localizer["CustomerMembershipRenewedSuccessfully"].Value);
        }
    }
}