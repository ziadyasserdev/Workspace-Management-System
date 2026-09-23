using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.PricingRule.Commands.UpdatePricingRule
{
    public class UpdatePricingRuleCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }

        public int PricingPlanId { get; set; }
        public int WorkspaceTypeId { get; set; }

        public PricingRuleType RuleType { get; set; }
        public decimal? Value { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }

        public bool IsActive { get; set; }
    }
}