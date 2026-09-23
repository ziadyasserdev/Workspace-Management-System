using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlanById
{
    public class GetPricingPlanByIdQuery
        : IRequest<Result<PricingPlanDto>>
    {
        public int Id { get; set; }
    }
}