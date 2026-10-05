using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.CustomerMemberships.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Queries.GetCurrentCustomerMembership
{
    public class GetCurrentCustomerMembershipQueryHandler
    : IRequestHandler<
        GetCurrentCustomerMembershipQuery,
        Result<CustomerMembershipDto>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetCurrentCustomerMembershipQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Result<CustomerMembershipDto>> Handle(
            GetCurrentCustomerMembershipQuery request,
            CancellationToken cancellationToken)
        {

            // Check Customer
            var customerExists =
                await _unitOfWork.Customers
                .Query()
                .AnyAsync(
                    x =>
                        x.Id == request.CustomerId &&
                        !x.IsDeleted,
                    cancellationToken);


            if (!customerExists)
            {
                return Result<CustomerMembershipDto>.Failure(
                    ResultStatus.NotFound,
                    "Customer not found.");
            }


            var now = DateTime.UtcNow;


            var membership =
                await _unitOfWork.CustomerMemberships
                .Query()
                .AsNoTracking()
                .Where(
                    x =>
                        x.CustomerId == request.CustomerId &&
                        !x.IsDeleted &&
                        x.Status == CustomerMembershipStatus.Active &&
                        x.StartDate <= now &&
                        x.EndDate >= now)
                .Select(
                    x => new CustomerMembershipDto
                    {
                        Id = x.Id,

                        MembershipId =
                            x.MembershipId,

                        MembershipName =
                            x.Membership.Name,

                        StartDate =
                            x.StartDate,

                        EndDate =
                            x.EndDate,

                        Status =
                            x.Status,

                        PricePaid =
                            x.PricePaid
                    })
                .FirstOrDefaultAsync(cancellationToken);


            if (membership == null)
            {
                return Result<CustomerMembershipDto>.Failure(
                    ResultStatus.NotFound,
                    "Customer has no active membership.");
            }


            return Result<CustomerMembershipDto>
                .Success(membership);
        }
    }
}
