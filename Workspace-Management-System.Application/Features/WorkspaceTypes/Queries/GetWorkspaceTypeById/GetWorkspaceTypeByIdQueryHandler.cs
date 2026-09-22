using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.WorkspaceTypes.Dtos;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Queries.GetWorkspaceTypeById
{
    public class GetWorkspaceTypeByIdQueryHandler
       : IRequestHandler<
           GetWorkspaceTypeByIdQuery,
           Result<WorkspaceTypeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWorkspaceTypeByIdQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<WorkspaceTypeDto>> Handle(
            GetWorkspaceTypeByIdQuery request,
            CancellationToken cancellationToken)
        {
            var workspaceType = await _unitOfWork.WorkspaceTypes
     .Query()
     .AsNoTracking()
     .Where(x => x.Id == request.Id && !x.IsDeleted)
     .Select(x => new WorkspaceTypeDto
     {
         Id = x.Id,
         Name = x.Name,
         Description = x.Description,
         IsActive = x.IsActive
     })
     .FirstOrDefaultAsync(cancellationToken);

            if (workspaceType is null)
            {
                return Result<WorkspaceTypeDto>.Failure(
                    ResultStatus.NotFound,
                    "Workspace type not found.");
            }

            return Result<WorkspaceTypeDto>.Success(
                workspaceType,
                "Workspace type retrieved successfully.");
        }
    }
}
