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
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.CreateWorkspaceType
{
    public class CreateWorkspaceTypeCommandHandler
        : IRequestHandler<CreateWorkspaceTypeCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer _localizer;

        public CreateWorkspaceTypeCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizerFactory factory)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = factory.Create(typeof(SharedResources));
        }

        public async Task<Result<int>> Handle(
            CreateWorkspaceTypeCommand request,
            CancellationToken cancellationToken)
        {
            var nameEn = request.NameEn.Trim();
            var nameAr = request.NameAr.Trim();

            var exists = await _unitOfWork.WorkspaceTypes
                .Query()
                .AnyAsync(
                    x => !x.IsDeleted &&
                         (x.NameEn == nameEn || x.NameAr == nameAr),
                    cancellationToken);

            if (exists)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    _localizer["WorkspaceTypeAlreadyExists"]);
            }

            var now = DateTime.UtcNow;

            var workspaceType = new WorkspaceType
            {
                NameEn = nameEn,
                NameAr = nameAr,
                DescriptionEn = request.DescriptionEn?.Trim(),
                DescriptionAr = request.DescriptionAr?.Trim(),
                IsActive = true,
                CreatedAt = now,
                CreatedBy = _currentUserService.UserId
            };

            await _unitOfWork.WorkspaceTypes.AddAsync(workspaceType);
            await _unitOfWork.SaveAsync();

            return Result<int>.Success(
                workspaceType.Id,
                _localizer["WorkspaceTypeCreatedSuccessfully"]);
        }
    }
}
