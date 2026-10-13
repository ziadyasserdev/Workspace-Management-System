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

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Commands.ResumeCustomerMembership
{
    public class ResumeCustomerMembershipCommandHandler
       : IRequestHandler<ResumeCustomerMembershipCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ResumeCustomerMembershipCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            ResumeCustomerMembershipCommand request,
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
                CustomerMembershipStatus.Suspended)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["OnlySuspendedMembershipsCanBeResumed"].Value);
            }

            var now = DateTime.UtcNow;

            if (customerMembership.EndDate <= now)
            {
                customerMembership.Status =
                    CustomerMembershipStatus.Expired;

                customerMembership.UpdatedAt = now;
                customerMembership.UpdatedBy = currentUserId;

                await _unitOfWork.SaveAsync();

                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["CustomerMembershipHasExpired"].Value);
            }

            customerMembership.Status =
                CustomerMembershipStatus.Active;

            customerMembership.UpdatedAt = now;
            customerMembership.UpdatedBy = currentUserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["CustomerMembershipResumedSuccessfully"].Value);
        }
    }
}
