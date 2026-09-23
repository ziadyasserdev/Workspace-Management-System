using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlans
{
    public class GetPricingPlansQueryHandler
        : IRequestHandler<GetPricingPlansQuery, Result<List<PricingPlanDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetPricingPlansQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<PricingPlanDto>>> Handle(
            GetPricingPlansQuery request,
            CancellationToken cancellationToken)
        {
            var query = unitOfWork.PricingPlans
                .Query()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Name.Contains(request.Search) ||
                    (x.Description != null &&
                     x.Description.Contains(request.Search)));
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(x =>
                    x.IsActive == request.IsActive.Value);
            }

            var pricingPlans = await query
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

            return Result<List<PricingPlanDto>>.Success(pricingPlans);
        }
    }
}