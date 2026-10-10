
namespace Workspace_Management_System.Application.Features.ProductCategories.DTOs;

public class ProductCategoryEditDto
{
    public int Id { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? DescriptionEn { get; set; }
    public string? DescriptionAr { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
}
