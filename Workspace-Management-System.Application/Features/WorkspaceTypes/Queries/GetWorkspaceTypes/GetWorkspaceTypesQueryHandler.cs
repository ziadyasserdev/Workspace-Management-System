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
using Workspace_Management_System.Application.Features.WorkspaceTypes.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Queries.GetWorkspaceTypes
{
    public class GetWorkspaceTypesQueryHandler
        : IRequestHandler<
            GetWorkspaceTypesQuery,
            Result<PaginatedResult<WorkspaceTypeDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer _localizer;

        public GetWorkspaceTypesQueryHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizerFactory factory)
        {
            _unitOfWork = unitOfWork;
            _localizer = factory.Create(typeof(SharedResources));
        }

        public async Task<Result<PaginatedResult<WorkspaceTypeDto>>> Handle(
            GetWorkspaceTypesQuery request,
            CancellationToken cancellationToken)
        {
            var isArabic =
                CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

            var query = _unitOfWork.WorkspaceTypes
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            // Search by the name in the selected language.
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = isArabic
                    ? query.Where(x => x.NameAr.Contains(search))
                    : query.Where(x => x.NameEn.Contains(search));
            }

            // Filter by active/inactive status.
            if (request.IsActive.HasValue)
            {
                query = query.Where(
                    x => x.IsActive == request.IsActive.Value);
            }

            // Count records after filtering, before pagination.
            var totalCount = await query.CountAsync(
                cancellationToken);

            // Apply sorting, pagination, and projection.
            var items = await query
                .OrderBy(x => isArabic ? x.NameAr : x.NameEn)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new WorkspaceTypeDto
                {
                    Id = x.Id,
                    Name = isArabic ? x.NameAr : x.NameEn,
                    Description = isArabic
                        ? x.DescriptionAr
                        : x.DescriptionEn,
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
                _localizer["WorkspaceTypesRetrievedSuccessfully"]);
        }
    }
}
