using MediatR;

namespace Workspace_Management_System.Application.Features.SessionProducts.Commands.RemoveProduct
{
    public class RemoveProductCommand : IRequest<Unit>
    {
        public int SessionId { get; set; }

        public int ProductId { get; set; }
    }
}