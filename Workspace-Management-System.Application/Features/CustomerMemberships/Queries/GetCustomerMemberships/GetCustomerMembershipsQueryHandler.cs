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

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Queries.GetCustomerMemberships
{
    public class GetCustomerMembershipsQueryHandler
     : IRequestHandler<
         GetCustomerMembershipsQuery,
         Result<List<CustomerMembershipDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetCustomerMembershipsQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Result<List<CustomerMembershipDto>>> Handle(
            GetCustomerMembershipsQuery request,
            CancellationToken cancellationToken)
        {

            // Check Customer Exists
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
                return Result<List<CustomerMembershipDto>>
                    .Failure(
                        ResultStatus.NotFound,
                        "Customer not found.");
            }


            var memberships =
                await _unitOfWork.CustomerMemberships
                    .Query()
                    .AsNoTracking()
                    .Where(
                        x =>
                            x.CustomerId == request.CustomerId &&
                            !x.IsDeleted)
                    .OrderByDescending(
                        x => x.StartDate)
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
                    .ToListAsync(cancellationToken);



            return Result<List<CustomerMembershipDto>>
                .Success(memberships);
        }
    }
}
