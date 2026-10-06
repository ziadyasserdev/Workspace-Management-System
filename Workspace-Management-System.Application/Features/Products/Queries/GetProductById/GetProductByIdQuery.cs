using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery
        : IRequest<Result<ProductDto>>
    {
        public int Id { get; set; }
    }
}