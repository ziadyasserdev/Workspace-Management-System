
namespace Workspace_Management_System.Application.Features.Services.Dtos;

public class ServiceEditDto
{
    public int Id { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? DescriptionEn { get; set; }
    public string? DescriptionAr { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}
