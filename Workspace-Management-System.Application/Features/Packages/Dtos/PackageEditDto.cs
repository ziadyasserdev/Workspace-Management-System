
namespace Workspace_Management_System.Application.Features.Packages.Dtos;

public class PackageEditDto
{
    public int Id { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? DescriptionEn { get; set; }
    public string? DescriptionAr { get; set; }
    public string PackageType { get; set; } = string.Empty;
    public decimal TotalHours { get; set; }
    public int? DurationDays { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}
