using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Sessions.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Sessions.Queries.GetActiveSessions
{
    public class GetActiveSessionsQueryHandler
         : IRequestHandler<
             GetActiveSessionsQuery,
             Result<PaginatedResult<ActiveSessionDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocalizationService _localizationService;

        public GetActiveSessionsQueryHandler(
            IUnitOfWork unitOfWork,
            ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _localizationService = localizationService;
        }

        public async Task<Result<PaginatedResult<ActiveSessionDto>>> Handle(
            GetActiveSessionsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Sessions
                .Query()
                .AsNoTracking()
                .Where(x =>
                    !x.IsDeleted &&
                    x.Status == SessionStatus.Active);

            // Filter by workspace
            if (request.WorkspaceId.HasValue)
            {
                query = query.Where(x =>
                    x.WorkspaceId == request.WorkspaceId.Value);
            }

            // Search by customer name in Arabic or English
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x =>
                    x.Customer.FullNameEn.Contains(search) ||
                    x.Customer.FullNameAr.Contains(search));
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Get current page
            var sessions = await query
                .OrderByDescending(x => x.StartTime)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ActiveSessionDto
                {
                    Id = x.Id,

                    CustomerId = x.CustomerId,
                    CustomerName =
                        _localizationService.GetLocalizedValue(
                            x.Customer.FullNameEn,
                            x.Customer.FullNameAr),

                    WorkspaceId = x.WorkspaceId,
                    WorkspaceName =
                        _localizationService.GetLocalizedValue(
                            x.Workspace.NameEn,
                            x.Workspace.NameAr),

                    WorkspaceCode = x.Workspace.Code,

                    PricingPlanId = x.PricingPlanId,
                    PricingPlanName =
                        _localizationService.GetLocalizedValue(
                            x.PricingPlan.NameEn,
                            x.PricingPlan.NameAr),

                    EmployeeId = x.EmployeeId,
                    EmployeeName =
                        _localizationService.GetLocalizedValue(
                            x.Employee.FirstNameEn + " " +
                            x.Employee.LastNameEn,
                            x.Employee.FirstNameAr + " " +
                            x.Employee.LastNameAr),

                    StartTime = x.StartTime,
                    NumberOfPeople = x.NumberOfPeople,
                    Status = x.Status
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<ActiveSessionDto>(
                sessions,
                totalCount,
                request.PageNumber,
                request.PageSize);

            return Result<PaginatedResult<ActiveSessionDto>>
                .Success(result);
        }
    }
}
