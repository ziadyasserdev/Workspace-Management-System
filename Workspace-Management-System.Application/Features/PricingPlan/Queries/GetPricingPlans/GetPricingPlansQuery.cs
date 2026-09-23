using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlans
{
    public class GetPricingPlansQuery
        : IRequest<Result<List<PricingPlanDto>>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public bool? IsActive { get; set; }
    }
}