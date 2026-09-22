using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Workspaces.Dtos;

namespace Workspace_Management_System.Application.Features.Workspaces.Queries.GetWorkspaces
{
    public class GetWorkspacesQueryHandler
      : IRequestHandler<
          GetWorkspacesQuery,
          Result<PaginatedResult<WorkspaceDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWorkspacesQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<WorkspaceDto>>> Handle(
            GetWorkspacesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Workspaces
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);
            

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    x.Code.Contains(search));
            }

           
            if (request.WorkspaceTypeId.HasValue)
            {
                query = query.Where(x =>
                    x.WorkspaceTypeId == request.WorkspaceTypeId.Value);
            }

         
            if (request.Status.HasValue)
            {
                query = query.Where(x =>
                    x.Status == request.Status.Value);
            }

         
            if (request.IsActive.HasValue)
            {
                query = query.Where(x =>
                    x.IsActive == request.IsActive.Value);
            }

            var totalCount = await query.CountAsync(
                cancellationToken);

            var items = await query
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
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
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<WorkspaceDto>(
                items,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result<PaginatedResult<WorkspaceDto>>.Success(
                result,
                "Workspaces retrieved successfully.");
        }
    }
}
