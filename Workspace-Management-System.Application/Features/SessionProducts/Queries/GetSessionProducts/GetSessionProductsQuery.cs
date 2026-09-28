using MediatR;
using Workspace_Management_System.Application.Features.SessionProducts.Dtos;

namespace Workspace_Management_System.Application.Features.SessionProducts.Queries.GetSessionProducts
{
    public class GetSessionProductsQuery
        : IRequest<List<SessionProductResponseDto>>
    {
        public int SessionId { get; set; }
    }
}