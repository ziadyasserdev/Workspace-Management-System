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
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Workspaces.Commands.CreateWorkspace
{

    public class CreateWorkspaceCommandHandler
      : IRequestHandler<CreateWorkspaceCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CreateWorkspaceCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(
            CreateWorkspaceCommand request,
            CancellationToken cancellationToken)
        {
            var workspaceType = await _unitOfWork.WorkspaceTypes
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.WorkspaceTypeId && !x.IsDeleted,
                    cancellationToken);

            if (workspaceType is null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    _localizer["WorkspaceTypeNotFound"].Value);
            }

            if (!workspaceType.IsActive)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    _localizer["CannotCreateWorkspaceForInactiveType"].Value);
            }

            var code = request.Code.Trim();

            var codeExists = await _unitOfWork.Workspaces
                .Query()
                .AnyAsync(
                    x => x.Code == code && !x.IsDeleted,
                    cancellationToken);

            if (codeExists)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    _localizer["WorkspaceCodeAlreadyExists"].Value);
            }

            var workspace = new Workspace
            {
                WorkspaceTypeId = request.WorkspaceTypeId,

                NameEn = request.NameEn.Trim(),
                NameAr = request.NameAr.Trim(),

                Code = code,
                Floor = request.Floor?.Trim(),
                Location = request.Location?.Trim(),

                Capacity = request.Capacity,
                Status = WorkspaceStatus.Available,

                DescriptionEn = request.DescriptionEn?.Trim(),
                DescriptionAr = request.DescriptionAr?.Trim(),

                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUser.UserId
            };

            await _unitOfWork.Workspaces.AddAsync(workspace);
            await _unitOfWork.SaveAsync();

            return Result<int>.Success(
                workspace.Id,
                _localizer["WorkspaceCreatedSuccessfully"].Value);
        }
    }
}
