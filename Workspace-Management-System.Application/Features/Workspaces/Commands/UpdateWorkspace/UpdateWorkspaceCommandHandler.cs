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

namespace Workspace_Management_System.Application.Features.Workspaces.Commands.UpdateWorkspace
{
    public class UpdateWorkspaceCommandHandler
          : IRequestHandler<UpdateWorkspaceCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public UpdateWorkspaceCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            UpdateWorkspaceCommand request,
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

            var workspaceType = await _unitOfWork.WorkspaceTypes
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.WorkspaceTypeId && !x.IsDeleted,
                    cancellationToken);

            if (workspaceType is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["WorkspaceTypeNotFound"].Value);
            }

            if (!workspaceType.IsActive)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["CannotAssignWorkspaceToInactiveType"].Value);
            }

            var code = request.Code.Trim();

            var codeExists = await _unitOfWork.Workspaces
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id &&
                         x.Code == code &&
                         !x.IsDeleted,
                    cancellationToken);

            if (codeExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["WorkspaceCodeAlreadyExists"].Value);
            }

            workspace.WorkspaceTypeId = request.WorkspaceTypeId;

            workspace.NameEn = request.NameEn.Trim();
            workspace.NameAr = request.NameAr.Trim();

            workspace.Code = code;
            workspace.Floor = request.Floor?.Trim();
            workspace.Location = request.Location?.Trim();

            workspace.Capacity = request.Capacity;

            workspace.DescriptionEn = request.DescriptionEn?.Trim();
            workspace.DescriptionAr = request.DescriptionAr?.Trim();

            workspace.UpdatedAt = DateTime.UtcNow;
            workspace.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["WorkspaceUpdatedSuccessfully"].Value);
        }
    }
}
