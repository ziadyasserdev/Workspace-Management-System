using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Workspaces.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Workspaces.Queries.GetWorkspaces
{
    public class GetWorkspacesQueryHandler
        : IRequestHandler<
            GetWorkspacesQuery,
            Result<PaginatedResult<WorkspaceListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public GetWorkspacesQueryHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<PaginatedResult<WorkspaceListDto>>> Handle(
            GetWorkspacesQuery request,
            CancellationToken cancellationToken)
        {
            var isArabic =
                CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

            var query = _unitOfWork.Workspaces
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    (isArabic
                        ? x.NameAr.Contains(search)
                        : x.NameEn.Contains(search))
                    || x.Code.Contains(search));
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

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(x => isArabic ? x.NameAr : x.NameEn)
                .ThenBy(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new WorkspaceListDto
                {
                    Id = x.Id,
                    WorkspaceTypeId = x.WorkspaceTypeId,

                    WorkspaceTypeName = isArabic
                        ? x.WorkspaceType.NameAr
                        : x.WorkspaceType.NameEn,

                    Name = isArabic
                        ? x.NameAr
                        : x.NameEn,

                    Code = x.Code,
                    Floor = x.Floor,
                    Location = x.Location,
                    Capacity = x.Capacity,
                    Status = x.Status,

                    Description = isArabic
                        ? x.DescriptionAr
                        : x.DescriptionEn,

                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<WorkspaceListDto>(
                items,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result<PaginatedResult<WorkspaceListDto>>.Success(
                result,
                _localizer["WorkspacesRetrievedSuccessfully"].Value);
        }
    }

}