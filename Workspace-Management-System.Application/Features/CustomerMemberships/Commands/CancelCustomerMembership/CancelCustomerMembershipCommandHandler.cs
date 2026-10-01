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

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Commands.CancelCustomerMembership
{
    public class CancelCustomerMembershipCommandHandler
     : IRequestHandler<
         CancelCustomerMembershipCommand,
         Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CancelCustomerMembershipCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(
            CancelCustomerMembershipCommand request,
            CancellationToken cancellationToken)
        {
           
            var currentUserId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return Result<bool>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

           
            var customerMembership =
                await _unitOfWork.CustomerMemberships
                    .Query()
                    .FirstOrDefaultAsync(
                        x =>
                            x.Id == request.Id &&
                            !x.IsDeleted,
                        cancellationToken);

            if (customerMembership == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Customer membership not found.");
            }

           
            if (customerMembership.Status ==
                CustomerMembershipStatus.Cancelled)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Customer membership is already cancelled.");
            }

            if (customerMembership.Status ==
                CustomerMembershipStatus.Expired)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Expired membership cannot be cancelled.");
            }

            
            var now = DateTime.UtcNow;

            customerMembership.Status =
                CustomerMembershipStatus.Cancelled;

            customerMembership.UpdatedAt = now;
            customerMembership.UpdatedBy = currentUserId;

          
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Customer membership cancelled successfully.");
        }
    }
}
