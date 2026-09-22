using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Workspaces.Commands.DeleteWorkspace
{
    public class DeleteWorkspaceCommandHandler
       : IRequestHandler<DeleteWorkspaceCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteWorkspaceCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            DeleteWorkspaceCommand request,
            CancellationToken cancellationToken)
        {
            var workspace = await _unitOfWork.Workspaces
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id &&
                         !x.IsDeleted,
                    cancellationToken);

            if (workspace is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Workspace not found.");
            }

            workspace.IsDeleted = true;
            workspace.IsActive = false;
            workspace.Status = Domain.Enums.WorkspaceStatus.Inactive;

            workspace.IsDeletedBy = _currentUser.UserId;
            workspace.UpdatedAt = DateTime.UtcNow;
            workspace.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Workspace deleted successfully.");
        }
    }
}
