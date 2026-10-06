namespace Workspace_Management_System.Application.Features.ProductCategories.DTOs
{
    public class ProductCategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}