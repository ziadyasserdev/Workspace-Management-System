using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.RestorePricingPlan
{
    public class RestorePricingPlanCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}