using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.UpdatePricingPlan
{
    public class UpdatePricingPlanCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}