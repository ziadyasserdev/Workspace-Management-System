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

namespace Workspace_Management_System.Application.Features.Workspaces.Commands.ChangeWorkspaceStatus
{
    public class ChangeWorkspaceStatusCommandHandler
       : IRequestHandler<
           ChangeWorkspaceStatusCommand,
           Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public ChangeWorkspaceStatusCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            ChangeWorkspaceStatusCommand request,
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

            if (!workspace.IsActive &&
                request.Status != Domain.Enums.WorkspaceStatus.Inactive)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Cannot change the status of an inactive workspace.");
            }

            if (workspace.Status == request.Status)
            {
                return Result<bool>.Success(
                    true,
                    $"Workspace is already {request.Status}.");
            }

            workspace.Status = request.Status;
            workspace.UpdatedAt = DateTime.UtcNow;
            workspace.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Workspace status changed successfully.");
        }
    }
}
