using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Services.Commands.DeleteService
{
    public class DeleteServiceCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}
