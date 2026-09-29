using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Sessions.Dtos;

namespace Workspace_Management_System.Application.Features.Sessions.Queries.GetSessionDetails
{
    public class GetSessionDetailsQueryHandler
     : IRequestHandler<GetSessionDetailsQuery, Result<SessionDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSessionDetailsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                    CustomerName =
                        x.Customer.FullName,

                    BookingId = x.BookingId,

                    WorkspaceId = x.WorkspaceId,
                    WorkspaceName = x.Workspace.Name,
                    WorkspaceCode = x.Workspace.Code,

                    PricingPlanId = x.PricingPlanId,
                    PricingPlanName = x.PricingPlan.Name,

                    EmployeeId = x.EmployeeId,
                    EmployeeName =
                        x.Employee.FirstName + " " +
                        x.Employee.LastName,

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
                            WorkspaceName = h.Workspace.Name,

                            StartTime = h.StartTime,
                            EndTime = h.EndTime,

                            EmployeeId = h.EmployeeId,
                            EmployeeName =
                                h.Employee.FirstName + " " +
                                h.Employee.LastName,

                            Reason = h.Reason
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (session is null)
            {
                return Result<SessionDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Session not found.");
            }

            return Result<SessionDetailsDto>.Success(session);
        }
    }
}
