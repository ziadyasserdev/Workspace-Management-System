using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Workspaces.Dtos;

namespace Workspace_Management_System.Application.Features.Workspaces.Queries.GetWorkspaceById
{
    public class GetWorkspaceByIdQueryHandler
         : IRequestHandler<
             GetWorkspaceByIdQuery,
             Result<WorkspaceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWorkspaceByIdQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<WorkspaceDto>> Handle(
            GetWorkspaceByIdQuery request,
            CancellationToken cancellationToken)
        {
            var workspace = await _unitOfWork.Workspaces
                .Query()
                .AsNoTracking()
                .Where(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted)
                .Select(x => new WorkspaceDto
                {
                    Id = x.Id,
                    WorkspaceTypeId = x.WorkspaceTypeId,
                    WorkspaceTypeName = x.WorkspaceType.Name,
                    Name = x.Name,
                    Code = x.Code,
                    Floor = x.Floor,
                    Location = x.Location,
                    Capacity = x.Capacity,
                    Status = x.Status,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (workspace is null)
            {
                return Result<WorkspaceDto>.Failure(
                    ResultStatus.NotFound,
                    "Workspace not found.");
            }

            return Result<WorkspaceDto>.Success(
                workspace,
                "Workspace retrieved successfully.");
        }
    }
}
