using MediatR;

namespace Workspace_Management_System.Application.Features.SessionProducts.Commands.ClearProducts
{
    public class ClearProductsCommand : IRequest<Unit>
    {
        public int SessionId { get; set; }
    }
}