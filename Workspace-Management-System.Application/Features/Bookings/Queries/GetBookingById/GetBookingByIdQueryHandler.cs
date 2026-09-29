using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Bookings.Dtos;

namespace Workspace_Management_System.Application.Features.Bookings.Queries.GetBookingById
{
    public class GetBookingByIdQueryHandler
    : IRequestHandler<GetBookingByIdQuery, Result<BookingDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBookingByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<BookingDetailsDto>> Handle(
            GetBookingByIdQuery request,
            CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Bookings
                .Query()
                .AsNoTracking()
                .Where(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted)
                .Select(x => new BookingDetailsDto
                {
                    Id = x.Id,

                    BookingNumber = x.BookingNumber,

                    CustomerId = x.CustomerId,

                    CustomerName =
                        x.Customer.FullName,

                    WorkspaceId = x.WorkspaceId,

                    WorkspaceName = x.Workspace.Name,

                    WorkspaceCode = x.Workspace.Code,

                    BookingDate = x.BookingDate,

                    StartTime = x.StartTime,

                    ExpectedEndTime = x.ExpectedEndTime,

                    NumberOfPeople = x.NumberOfPeople,

                    Status = x.Status,

                    Notes = x.Notes,

                    Session = x.Session == null
                        ? null
                        : new SessionSummaryDto
                        {
                            Id = x.Session.Id,

                            StartTime = x.Session.StartTime,

                            EndTime = x.Session.EndTime,

                            Status = x.Session.Status,

                            EmployeeId = x.Session.EmployeeId,

                            WorkspaceId = x.Session.WorkspaceId,

                            PricingPlanId = x.Session.PricingPlanId
                        }
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (booking == null)
            {
                return Result<BookingDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Booking not found.");
            }

            return Result<BookingDetailsDto>.Success(booking);
        }
    }
}
