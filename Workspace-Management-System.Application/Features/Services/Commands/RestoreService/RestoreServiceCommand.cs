using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Services.Commands.RestoreService
{
    public class RestoreServiceCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}
