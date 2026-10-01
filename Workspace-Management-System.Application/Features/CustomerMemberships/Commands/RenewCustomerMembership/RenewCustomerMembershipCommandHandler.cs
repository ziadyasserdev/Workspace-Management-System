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
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Commands.RenewCustomerMembership
{
    public class RenewCustomerMembershipCommandHandler
    : IRequestHandler<
        RenewCustomerMembershipCommand,
        Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RenewCustomerMembershipCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
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
                    "User is not authenticated.");
            }

        
            var oldMembership =
                await _unitOfWork.CustomerMemberships
                    .Query()
                    .FirstOrDefaultAsync(
                        x =>
                            x.Id == request.Id &&
                            !x.IsDeleted,
                        cancellationToken);

            if (oldMembership == null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Customer membership not found.");
            }

         
            if (oldMembership.Status ==
                    CustomerMembershipStatus.Active ||
                oldMembership.Status ==
                    CustomerMembershipStatus.Suspended)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Active or suspended memberships cannot be renewed.");
            }

         
            var membership =
                await _unitOfWork.Memberships
                    .Query()
                    .FirstOrDefaultAsync(
                        x =>
                            x.Id == oldMembership.MembershipId &&
                            !x.IsDeleted,
                        cancellationToken);

            if (membership == null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Membership plan not found.");
            }

        
            if (!membership.IsActive)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "This membership plan is currently inactive.");
            }

           
            var hasCurrentMembership =
                await _unitOfWork.CustomerMemberships
                    .Query()
                    .AnyAsync(
                        x =>
                            x.CustomerId == oldMembership.CustomerId &&
                            !x.IsDeleted &&
                            (x.Status ==
                                CustomerMembershipStatus.Active ||
                             x.Status ==
                                CustomerMembershipStatus.Suspended),
                        cancellationToken);

            if (hasCurrentMembership)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Customer already has an active or suspended membership.");
            }

           
            var now = DateTime.UtcNow;

            var newMembership = new CustomerMembership
            {
                CustomerId = oldMembership.CustomerId,
                MembershipId = oldMembership.MembershipId,

                StartDate = now,

                EndDate = now.AddDays(
                    membership.DurationDays),

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
                "Customer membership renewed successfully.");
        }
    }
}
