using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler
      : IRequestHandler<CreateBookingCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CreateBookingCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(
            CreateBookingCommand request,
            CancellationToken cancellationToken)
        {
            await using var transaction =
                await _unitOfWork.BeginTransactionAsync();

            try
            {
                var currentUserId = _currentUserService.UserId;

                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    return Result<int>.Failure(
                        ResultStatus.Unauthorized,
                        _localizer["UserNotAuthenticated"].Value);
                }

                var now = DateTime.UtcNow;

                if (request.StartTime <= now)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        _localizer["BookingStartTimeMustBeInFuture"].Value);
                }

                if (request.ExpectedEndTime.HasValue &&
                    request.ExpectedEndTime.Value <= request.StartTime)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        _localizer["ExpectedEndTimeAfterStartTime"].Value);
                }

                if (request.BookingDate.Date != request.StartTime.Date)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        _localizer["BookingDateMustMatchStartDate"].Value);
                }

                var customer = await _unitOfWork.Customers
                    .Query()
                    .FirstOrDefaultAsync(
                        x => x.Id == request.CustomerId && !x.IsDeleted,
                        cancellationToken);

                if (customer is null)
                {
                    return Result<int>.Failure(
                        ResultStatus.NotFound,
                        _localizer["CustomerNotFound"].Value);
                }

                var workspace = await _unitOfWork.Workspaces
                    .Query()
                    .FirstOrDefaultAsync(
                        x => x.Id == request.WorkspaceId && !x.IsDeleted,
                        cancellationToken);

                if (workspace is null)
                {
                    return Result<int>.Failure(
                        ResultStatus.NotFound,
                        _localizer["WorkspaceNotFound"].Value);
                }

                if (!workspace.IsActive)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        _localizer["WorkspaceIsInactive"].Value);
                }

                if (workspace.Status == WorkspaceStatus.Maintenance)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        _localizer["WorkspaceUnderMaintenance"].Value);
                }

                if (request.NumberOfPeople > workspace.Capacity)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        _localizer["NumberOfPeopleExceedsWorkspaceCapacity"].Value);
                }

                var newStart = request.StartTime;
                var newEnd = request.ExpectedEndTime;

                var hasConflict = await _unitOfWork.Bookings
                    .Query()
                    .AnyAsync(
                        x =>
                            !x.IsDeleted &&
                            x.WorkspaceId == request.WorkspaceId &&
                            x.Status != BookingStatus.Cancelled &&
                            x.Status != BookingStatus.NoShow &&
                            x.Status != BookingStatus.Completed &&
                            (
                                newEnd == null
                                    ? x.ExpectedEndTime == null
                                        ? x.StartTime == newStart
                                        : x.StartTime < newStart &&
                                          x.ExpectedEndTime > newStart
                                    : x.StartTime < newEnd &&
                                      (
                                          x.ExpectedEndTime == null ||
                                          x.ExpectedEndTime > newStart
                                      )
                            ),
                        cancellationToken);

                if (hasConflict)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        _localizer["WorkspaceHasConflictingBooking"].Value);
                }

                var bookingNumber = $"BK-{DateTime.UtcNow:yyyyMMddHHmmssfff}";

                var booking = new Booking
                {
                    BookingNumber = bookingNumber,
                    CustomerId = request.CustomerId,
                    WorkspaceId = request.WorkspaceId,
                    BookingDate = request.BookingDate.Date,
                    StartTime = request.StartTime,
                    ExpectedEndTime = request.ExpectedEndTime,
                    NumberOfPeople = request.NumberOfPeople,
                    Status = BookingStatus.Pending,

                    Notes = string.IsNullOrWhiteSpace(request.Notes)
                        ? null
                        : request.Notes.Trim(),

                    CreatedAt = now,
                    CreatedBy = currentUserId
                };

                await _unitOfWork.Bookings.AddAsync(booking);
                await _unitOfWork.SaveAsync();

                await transaction.CommitAsync(cancellationToken);

                return Result<int>.Success(
                    booking.Id,
                    _localizer["BookingCreatedSuccessfully"].Value);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
  