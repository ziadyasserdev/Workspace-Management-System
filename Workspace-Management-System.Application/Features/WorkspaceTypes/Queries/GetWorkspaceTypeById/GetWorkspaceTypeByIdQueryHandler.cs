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
using Workspace_Management_System.Application.Features.WorkspaceTypes.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Queries.GetWorkspaceTypeById
{
    public class GetWorkspaceTypeByIdQueryHandler
         : IRequestHandler<
             GetWorkspaceTypeByIdQuery,
             Result<WorkspaceTypeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public GetWorkspaceTypeByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
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

                    NameEn = x.NameEn,
                    NameAr = x.NameAr,

                    DescriptionEn = x.DescriptionEn,
                    DescriptionAr = x.DescriptionAr,

                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (workspaceType is null)
            {
                return Result<WorkspaceTypeDto>.Failure(
                    ResultStatus.NotFound,
                    _localizer["WorkspaceTypeNotFound"].Value);
            }

            return Result<WorkspaceTypeDto>.Success(
                workspaceType,
                _localizer["WorkspaceTypeRetrievedSuccessfully"].Value);
        }
    }
}
