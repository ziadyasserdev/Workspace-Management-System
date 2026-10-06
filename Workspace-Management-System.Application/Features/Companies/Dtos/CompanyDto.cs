namespace Workspace_Management_System.Application.Features.Companies.DTOs
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string ContactPerson { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public string? TaxNumber { get; set; }
        public string? TaxInformationEn { get; set; }
        public string? TaxInformationAr { get; set; }
        public string? ContractDetailsEn { get; set; }
        public string? ContractDetailsAr { get; set; }
        public int? PricingPlanId { get; set; }
        public decimal CreditLimit { get; set; }
        public bool IsActive { get; set; }
    }
}