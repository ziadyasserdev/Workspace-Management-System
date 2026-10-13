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

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Commands.SuspendCustomerMembership
{
    public class SuspendCustomerMembershipCommandHandler
      : IRequestHandler<SuspendCustomerMembershipCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public SuspendCustomerMembershipCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            SuspendCustomerMembershipCommand request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return Result<bool>.Failure(
                    ResultStatus.Unauthorized,
                    _localizer["UserNotAuthenticated"].Value);
            }

            var customerMembership =
                await _unitOfWork.CustomerMemberships
                    .Query()
                    .FirstOrDefaultAsync(
                        x => x.Id == request.Id && !x.IsDeleted,
                        cancellationToken);

            if (customerMembership == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["CustomerMembershipNotFound"].Value);
            }

            if (customerMembership.Status !=
                CustomerMembershipStatus.Active)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["OnlyActiveMembershipsCanBeSuspended"].Value);
            }

            var now = DateTime.UtcNow;

            customerMembership.Status =
                CustomerMembershipStatus.Suspended;

            customerMembership.UpdatedAt = now;
            customerMembership.UpdatedBy = currentUserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["CustomerMembershipSuspendedSuccessfully"].Value);
        }
    }
}
