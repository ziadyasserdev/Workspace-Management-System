using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Workspaces.Commands.ChangeWorkspaceStatus
{
    public class ChangeWorkspaceStatusCommandHandler
        : IRequestHandler<ChangeWorkspaceStatusCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ChangeWorkspaceStatusCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            ChangeWorkspaceStatusCommand request,
            CancellationToken cancellationToken)
        {
            var workspace = await _unitOfWork.Workspaces
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.IsDeleted,
                    cancellationToken);

            if (workspace is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["WorkspaceNotFound"].Value);
            }

            if (!workspace.IsActive &&
                request.Status != WorkspaceStatus.Inactive)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["CannotChangeStatusOfInactiveWorkspace"].Value);
            }

            if (workspace.Status == request.Status)
            {
                return Result<bool>.Success(
                    true,
                    _localizer[$"WorkspaceStatusAlready{request.Status}"].Value);
            }

            workspace.Status = request.Status;
            workspace.UpdatedAt = DateTime.UtcNow;
            workspace.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["WorkspaceStatusChangedSuccessfully"].Value);
        }
    }

}
