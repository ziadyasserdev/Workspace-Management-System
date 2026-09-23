namespace Workspace_Management_System.Application.Features.PricingPlan.Queries
{
    public class PricingPlanDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}