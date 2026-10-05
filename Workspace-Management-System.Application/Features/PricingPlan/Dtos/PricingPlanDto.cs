namespace Workspace_Management_System.Application.Features.PricingPlan.Queries
{
    public class PricingPlanDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public bool IsActive { get; set; }
    }
}