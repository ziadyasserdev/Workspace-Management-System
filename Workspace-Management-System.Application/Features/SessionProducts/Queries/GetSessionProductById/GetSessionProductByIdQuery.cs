using MediatR;
using Workspace_Management_System.Application.Features.SessionProducts.Dtos;

namespace Workspace_Management_System.Application.Features.SessionProducts.Queries.GetSessionProductById;

public class GetSessionProductByIdQuery : IRequest<SessionProductEditDto>
{
    public int Id { get; set; }
}