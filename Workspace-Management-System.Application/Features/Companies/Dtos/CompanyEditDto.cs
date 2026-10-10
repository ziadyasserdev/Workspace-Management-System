namespace Workspace_Management_System.Application.Features.Companies.DTOs;

public class CompanyEditDto
{
    public int Id { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? TaxNumber { get; set; }
    public string? TaxInformationEn { get; set; }
    public string? TaxInformationAr { get; set; }
    public string? ContractDetailsEn { get; set; }
    public string? ContractDetailsAr { get; set; }
    public int? PricingPlanId { get; set; }
    public decimal? CreditLimit { get; set; }
    public bool IsActive { get; set; }
}