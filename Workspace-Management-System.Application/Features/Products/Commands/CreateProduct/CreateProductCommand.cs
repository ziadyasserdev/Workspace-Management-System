using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Result<int>>
    {
        public int ProductCategoryId { get; set; }

        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;

        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }

        public string Sku { get; set; } = null!;

        public decimal SellingPrice { get; set; }

        public decimal CostPrice { get; set; }

        public bool IsActive { get; set; } = true;
    }
}