using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Services.Dtos;

namespace Workspace_Management_System.Application.Features.Services.Queries.GetServiceById
{
    public class GetServiceByIdQuery : IRequest<Result<ServiceResponseDto>>
    {
        public int Id { get; set; }
    }
}
