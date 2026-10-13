using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Workspaces.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Workspaces.Queries.GetWorkspaceById
{
    public class GetWorkspaceByIdQueryHandler
        : IRequestHandler<GetWorkspaceByIdQuery, Result<WorkspaceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public GetWorkspaceByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<WorkspaceDto>> Handle(
            GetWorkspaceByIdQuery request,
            CancellationToken cancellationToken)
        {
            var workspace = await _unitOfWork.Workspaces
                .Query()
                .AsNoTracking()
                .Where(x => x.Id == request.Id && !x.IsDeleted)
                .Select(x => new WorkspaceDto
                {
                    Id = x.Id,

                    WorkspaceTypeId = x.WorkspaceTypeId,
                    WorkspaceTypeNameEn = x.WorkspaceType.NameEn,
                    WorkspaceTypeNameAr = x.WorkspaceType.NameAr,

                    NameEn = x.NameEn,
                    NameAr = x.NameAr,

                    Code = x.Code,
                    Floor = x.Floor,
                    Location = x.Location,
                    Capacity = x.Capacity,
                    Status = x.Status,

                    DescriptionEn = x.DescriptionEn,
                    DescriptionAr = x.DescriptionAr,

                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (workspace is null)
            {
                return Result<WorkspaceDto>.Failure(
                    ResultStatus.NotFound,
                    _localizer["WorkspaceNotFound"].Value);
            }

            return Result<WorkspaceDto>.Success(
                workspace,
                _localizer["WorkspaceRetrievedSuccessfully"].Value);
        }
    }
}
