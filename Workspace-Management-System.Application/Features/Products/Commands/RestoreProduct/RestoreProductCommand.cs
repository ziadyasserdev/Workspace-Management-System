using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Products.Commands.RestoreProduct
{
    public class RestoreProductCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}