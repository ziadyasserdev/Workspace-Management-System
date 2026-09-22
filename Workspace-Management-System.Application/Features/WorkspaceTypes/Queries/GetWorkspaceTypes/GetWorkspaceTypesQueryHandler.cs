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
using Workspace_Management_System.Application.Features.WorkspaceTypes.Dtos;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Queries.GetWorkspaceTypes
{
    public class GetWorkspaceTypesQueryHandler
        : IRequestHandler<
            GetWorkspaceTypesQuery,
            Result<PaginatedResult<WorkspaceTypeDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWorkspaceTypesQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<WorkspaceTypeDto>>> Handle(
            GetWorkspaceTypesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.WorkspaceTypes
     .Query()
     .AsNoTracking()
     .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    x.Name.Contains(search));
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
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new WorkspaceTypeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<WorkspaceTypeDto>(
                items,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result<PaginatedResult<WorkspaceTypeDto>>.Success(
                result,
                "Workspace types retrieved successfully.");
        }
    }
}
