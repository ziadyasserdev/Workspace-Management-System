using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.CreateWorkspaceType
{
    public class CreateWorkspaceTypeCommand : IRequest<Result<int>>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
