using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Memberships.Dtos;

namespace Workspace_Management_System.Application.Features.Memberships.Queries.GetMemberships
{
    public class GetMembershipsQueryHandler
         : IRequestHandler<
             GetMembershipsQuery,
             Result<PaginatedResult<MembershipListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMembershipsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<MembershipListDto>>> Handle(
            GetMembershipsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Memberships
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

          
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    (x.Description != null &&
                     x.Description.Contains(search)));
            }

          
            if (request.IsActive.HasValue)
            {
                query = query.Where(x =>
                    x.IsActive == request.IsActive.Value);
            }

            var totalCount = await query.CountAsync(
                cancellationToken);

          
            query = request.SortDescending
                ? query.OrderByDescending(x => x.CreatedAt)
                : query.OrderBy(x => x.CreatedAt);

       
            var items = await query
                .Select(x => new MembershipListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    DurationDays = x.DurationDays,
                    Price = x.Price,
                    IsActive = x.IsActive,

                    BenefitsCount = x.Benefits
                        .Count(b => !b.IsDeleted),

                    ActiveCustomerMembershipsCount =
                        x.CustomerMemberships.Count(cm =>
                            !cm.IsDeleted &&
                            cm.Status ==
                                Domain.Enums.CustomerMembershipStatus.Active)
                })
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<MembershipListDto>(
                items,
                totalCount,
                request.PageNumber,
                request.PageSize);

            return Result<PaginatedResult<MembershipListDto>>
                .Success(result);
        }
    }
}
