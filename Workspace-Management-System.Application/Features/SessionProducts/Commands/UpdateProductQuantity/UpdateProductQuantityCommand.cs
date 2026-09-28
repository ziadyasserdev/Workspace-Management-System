using MediatR;
using Workspace_Management_System.Application.Features.SessionProducts.Dtos;

namespace Workspace_Management_System.Application.Features.SessionProducts.Commands.UpdateProductQuantity
{
    public class UpdateProductQuantityCommand
        : IRequest<SessionProductResponseDto>
    {
        public int SessionId { get; set; }

        public int ProductId { get; set; }

        public decimal Quantity { get; set; }
    }
}