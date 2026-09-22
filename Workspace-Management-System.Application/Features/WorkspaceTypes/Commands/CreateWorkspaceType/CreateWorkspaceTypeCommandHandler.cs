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
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.CreateWorkspaceType
{
    public class CreateWorkspaceTypeCommandHandler
    : IRequestHandler<CreateWorkspaceTypeCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateWorkspaceTypeCommandHandler(
            IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            CreateWorkspaceTypeCommand request,
            CancellationToken cancellationToken)
        {
            var name = request.Name.Trim();

            var exists = await _unitOfWork.WorkspaceTypes
        .Query()
        .AnyAsync(
            x => x.Name == name &&
                 !x.IsDeleted,
            cancellationToken);

            if (exists)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Workspace type already exists.");
            }

            var workspaceType = new WorkspaceType
            {
                Name = name,
                Description = request.Description?.Trim(),
                IsActive = true,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserService.UserId
            };

            await _unitOfWork.WorkspaceTypes
                .AddAsync(workspaceType);

            await _unitOfWork.SaveAsync();

            return Result<int>.Success(
                workspaceType.Id,
                "Workspace type created successfully.");
        }
    }
}
