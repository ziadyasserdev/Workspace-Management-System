using MediatR;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.PricingRules.Dtos;
using Workspace_Management_System.Domain.Enums;

public class GetPricingRulesQuery
    : IRequest<Result<PaginatedResult<PricingRuleDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public int? PricingPlanId { get; set; }
    public int? WorkspaceTypeId { get; set; }

    public PricingRuleType? RuleType { get; set; }

    public bool? IsActive { get; set; }
}