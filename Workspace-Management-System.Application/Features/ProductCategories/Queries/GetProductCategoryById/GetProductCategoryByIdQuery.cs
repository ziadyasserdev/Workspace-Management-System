using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.ProductCategories.DTOs;

namespace Workspace_Management_System.Application.Features.ProductCategories.Queries.GetProductCategoryById
{
    public class GetProductCategoryByIdQuery
        : IRequest<Result<ProductCategoryDto>>
    {
        public int Id { get; set; }
    }
}