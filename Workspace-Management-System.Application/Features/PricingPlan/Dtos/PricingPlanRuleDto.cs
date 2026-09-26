namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlanRules;

public class PricingPlanRuleDto
{
    public int Id { get; set; }

    public int PricingPlanId { get; set; }

    public int WorkspaceTypeId { get; set; }

    public string RuleType { get; set; } = null!;

    public decimal? Value { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? DayOfWeek { get; set; }

    public bool IsActive { get; set; }
}