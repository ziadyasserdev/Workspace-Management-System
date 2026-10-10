
namespace Workspace_Management_System.Application.Features.Products.DTOs;

public class ProductEditDto
{
    public int Id { get; set; }
    public int ProductCategoryId { get; set; }
    public string ProductCategoryNameEn { get; set; } = string.Empty;
    public string ProductCategoryNameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string? DescriptionEn { get; set; }
    public string? DescriptionAr { get; set; }
    public string Sku { get; set; } = string.Empty;
    public decimal SellingPrice { get; set; }
    public decimal CostPrice { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
}
