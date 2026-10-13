using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Sessions.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Sessions.Queries.GetSessionDetails
{
    public class GetSessionDetailsQueryHandler
        : IRequestHandler<GetSessionDetailsQuery, Result<SessionDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public GetSessionDetailsQueryHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<SessionDetailsDto>> Handle(
            GetSessionDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var session = await _unitOfWork.Sessions
                .Query()
                .AsNoTracking()
                .Where(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted)
                .Select(x => new SessionDetailsDto
                {
                    Id = x.Id,

                    CustomerId = x.CustomerId,
                    CustomerNameEn = x.Customer.FullNameEn,
                    CustomerNameAr = x.Customer.FullNameAr,

                    BookingId = x.BookingId,

                    WorkspaceId = x.WorkspaceId,
                    WorkspaceNameEn = x.Workspace.NameEn,
                    WorkspaceNameAr = x.Workspace.NameAr,
                    WorkspaceCode = x.Workspace.Code,

                    PricingPlanId = x.PricingPlanId,
                    PricingPlanNameEn = x.PricingPlan.NameEn,
                    PricingPlanNameAr = x.PricingPlan.NameAr,

                    EmployeeId = x.EmployeeId,
                    EmployeeNameEn =
                        x.Employee.FirstNameEn + " " +
                        x.Employee.LastNameEn,
                    EmployeeNameAr =
                        x.Employee.FirstNameAr + " " +
                        x.Employee.LastNameAr,

                    StartTime = x.StartTime,
                    EndTime = x.EndTime,

                    NumberOfPeople = x.NumberOfPeople,
                    Status = x.Status,

                    WorkspaceHistory = x.WorkspaceHistory
                        .Where(h => !h.IsDeleted)
                        .OrderBy(h => h.StartTime)
                        .Select(h => new SessionWorkspaceHistoryDto
                        {
                            Id = h.Id,

                            WorkspaceId = h.WorkspaceId,
                            WorkspaceNameEn = h.Workspace.NameEn,
                            WorkspaceNameAr = h.Workspace.NameAr,

                            StartTime = h.StartTime,
                            EndTime = h.EndTime,

                            EmployeeId = h.EmployeeId,
                            EmployeeNameEn =
                                h.Employee.FirstNameEn + " " +
                                h.Employee.LastNameEn,
                            EmployeeNameAr =
                                h.Employee.FirstNameAr + " " +
                                h.Employee.LastNameAr,

                            Reason = h.Reason
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (session is null)
            {
                return Result<SessionDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    _localizer["SessionNotFound"].Value);
            }

            return Result<SessionDetailsDto>.Success(session);
        }
    }
}
