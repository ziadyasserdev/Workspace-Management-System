using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlans
{
    public class GetPricingPlansQueryHandler
        : IRequestHandler<
            GetPricingPlansQuery,
            Result<PaginatedResult<PricingPlanDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetPricingPlansQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<PricingPlanDto>>> Handle(
            GetPricingPlansQuery request,
            CancellationToken cancellationToken)
        {
            var query = unitOfWork.PricingPlans
                .Query()
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

            var pricingPlans = await query
                .OrderBy(x => x.Name)
                .Select(x => new PricingPlanDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<PricingPlanDto>(
                pricingPlans,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result<PaginatedResult<PricingPlanDto>>.Success(
                result);
        }
    }
}