using MediatR;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Products.DTOs;

namespace Workspace_Management_System.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQuery
        : IRequest<Result<PaginatedResult<ProductDto>>>
    {
        public string? Search { get; set; }

        public int? ProductCategoryId { get; set; }

        public bool? IsActive { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}