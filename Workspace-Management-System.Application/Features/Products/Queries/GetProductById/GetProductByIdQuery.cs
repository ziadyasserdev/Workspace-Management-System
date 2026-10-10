using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Products.DTOs;

namespace Workspace_Management_System.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery
        : IRequest<Result<ProductEditDto>>
    {
        public int Id { get; set; }
    }
}