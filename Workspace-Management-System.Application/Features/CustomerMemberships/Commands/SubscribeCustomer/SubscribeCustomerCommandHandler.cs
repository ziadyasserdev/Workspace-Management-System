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

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Commands.SubscribeCustomer
{
    public class SubscribeCustomerCommandHandler
      : IRequestHandler<SubscribeCustomerCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public SubscribeCustomerCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            SubscribeCustomerCommand request,
            CancellationToken cancellationToken)
        {
          
            var currentUserId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

          
            var customer = await _unitOfWork.Customers
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.CustomerId &&
                         !x.IsDeleted,
                    cancellationToken);

            if (customer == null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Customer not found.");
            }

           
            var membership = await _unitOfWork.Memberships
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.MembershipId &&
                         !x.IsDeleted,
                    cancellationToken);

            if (membership == null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Membership not found.");
            }

           
            if (!membership.IsActive)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "This membership is currently inactive.");
            }

          
            var existingMembership =
                await _unitOfWork.CustomerMemberships
                    .Query()
                    .AnyAsync(
                        x => x.CustomerId == request.CustomerId &&
                             !x.IsDeleted &&
                             (x.Status == CustomerMembershipStatus.Active ||
                              x.Status == CustomerMembershipStatus.Suspended),
                        cancellationToken);

            if (existingMembership)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Customer already has an active or suspended membership.");
            }

        
            var now = DateTime.UtcNow;

            var customerMembership = new CustomerMembership
            {
                CustomerId = request.CustomerId,
                MembershipId = request.MembershipId,

                StartDate = now,

                EndDate = now.AddDays(
                    membership.DurationDays),

                Status = CustomerMembershipStatus.Active,

              
                PricePaid = membership.Price,

                CreatedAt = now,
                CreatedBy = currentUserId
            };

            
            await _unitOfWork.CustomerMemberships
                .AddAsync(customerMembership);

           
            await _unitOfWork.SaveAsync();

            
            return Result<int>.Success(
                customerMembership.Id,
                "Customer subscribed successfully.");
        }
    }
}
