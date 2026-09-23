using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.PricingPlan.Queries;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlanById
{
    public class GetPricingPlanByIdQueryHandler
        : IRequestHandler<GetPricingPlanByIdQuery, Result<PricingPlanDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetPricingPlanByIdQueryHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<PricingPlanDto>> Handle(
            GetPricingPlanByIdQuery request,
            CancellationToken cancellationToken)
        {
            var pricingPlan = await unitOfWork.PricingPlans
                .Query()
                .Where(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted)
                .Select(x => new PricingPlanDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (pricingPlan == null)
            {
                return Result<PricingPlanDto>.Failure(
                    ResultStatus.NotFound,
                    "Pricing plan not found.");
            }

            return Result<PricingPlanDto>.Success(
                pricingPlan);
        }
    }
}