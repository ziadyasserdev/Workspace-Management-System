namespace Workspace_Management_System.Application.Features.Products.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        public int ProductCategoryId { get; set; }

        public string? ProductCategoryName { get; set; }

        public string EnglishName { get; set; } = null!;

        public string? Description { get; set; }

        public string Sku { get; set; } = null!;

        public decimal SellingPrice { get; set; }

        public decimal CostPrice { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}