
namespace Workspace_Management_System.Application.Features.PricingRules.Dtos;

public class PricingRuleEditDto
{
    public int Id { get; set; }
    public int PricingPlanId { get; set; }
    public int WorkspaceTypeId { get; set; }
    public string RuleType { get; set; } = string.Empty;
    public decimal? Value { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? DayOfWeek { get; set; }
    public bool IsActive { get; set; }
}
