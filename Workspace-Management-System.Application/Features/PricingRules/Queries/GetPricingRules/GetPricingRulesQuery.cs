using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.PricingRules.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.PricingRule.Queries.GetPricingRules
{
    public class GetPricingRulesQuery
        : IRequest<Result<List<PricingRuleDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public int? PricingPlanId { get; set; }
        public int? WorkspaceTypeId { get; set; }

        public PricingRuleType? RuleType { get; set; }

        public bool? IsActive { get; set; }
    }
}