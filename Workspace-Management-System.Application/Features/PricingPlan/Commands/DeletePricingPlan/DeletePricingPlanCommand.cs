using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.DeletePricingPlan
{
    public class DeletePricingPlanCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}