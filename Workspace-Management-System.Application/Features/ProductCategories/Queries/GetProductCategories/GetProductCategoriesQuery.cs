using MediatR;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.ProductCategories.DTOs;

namespace Workspace_Management_System.Application.Features.ProductCategories.Queries.GetProductCategories
{
    public class GetProductCategoriesQuery
        : IRequest<Result<PaginatedResult<ProductCategoryDto>>>
    {
        public string? Search { get; set; }

        public bool? IsActive { get; set; }

        public bool IncludeDeleted { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}