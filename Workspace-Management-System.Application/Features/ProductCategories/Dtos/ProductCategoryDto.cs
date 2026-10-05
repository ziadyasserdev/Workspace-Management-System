namespace Workspace_Management_System.Application.Features.ProductCategories.DTOs
{
    public class ProductCategoryDto
    {
        public int Id { get; set; }

        public string NameEn { get; set; } = null!;

        public string NameAr { get; set; } = null!;

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}