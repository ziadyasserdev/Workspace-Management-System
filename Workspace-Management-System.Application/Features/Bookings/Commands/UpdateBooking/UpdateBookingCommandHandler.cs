using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Bookings.Commands.UpdateBooking
{
    public class UpdateBookingCommandHandler
    : IRequestHandler<UpdateBookingCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateBookingCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(
            UpdateBookingCommand request,
            CancellationToken cancellationToken)
        {
         
            if (!_currentUserService.IsAuthenticated)
            {
                return Result<bool>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

        
            var booking = await _unitOfWork.Bookings
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id &&
                         !x.IsDeleted,
                    cancellationToken);

            if (booking == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Booking not found.");
            }

            
            if (booking.Status != BookingStatus.Pending &&
                booking.Status != BookingStatus.Confirmed)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    $"Booking cannot be updated while its status is {booking.Status}.");
            }

          
            if (request.StartTime <= DateTime.UtcNow)
            {
                return Result<bool>.Failure(
                    ResultStatus.ValidationError,
                    "Booking start time must be in the future.");
            }

            if (request.ExpectedEndTime.HasValue &&
                request.ExpectedEndTime.Value <= request.StartTime)
            {
                return Result<bool>.Failure(
                    ResultStatus.ValidationError,
                    "Expected end time must be after start time.");
            }

            if (request.BookingDate.Date != request.StartTime.Date)
            {
                return Result<bool>.Failure(
                    ResultStatus.ValidationError,
                    "Booking date must match the start time date.");
            }

           
            var customer = await _unitOfWork.Customers
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.CustomerId &&
                         !x.IsDeleted,
                    cancellationToken);

            if (customer == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Customer not found.");
            }

           
            var workspace = await _unitOfWork.Workspaces
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.WorkspaceId &&
                         !x.IsDeleted,
                    cancellationToken);

            if (workspace == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Workspace not found.");
            }

        
            if (!workspace.IsActive)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Workspace is inactive.");
            }

            if (workspace.Status == WorkspaceStatus.Maintenance ||
                workspace.Status == WorkspaceStatus.Inactive)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Workspace is not available for booking.");
            }

          
            if (request.NumberOfPeople > workspace.Capacity)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Number of people exceeds workspace capacity.");
            }

          
            var hasConflict = await _unitOfWork.Bookings
                .Query()
                .AnyAsync(x =>
                    x.Id != booking.Id &&
                    !x.IsDeleted &&
                    x.WorkspaceId == request.WorkspaceId &&

                    x.Status != BookingStatus.Cancelled &&
                    x.Status != BookingStatus.NoShow &&
                    x.Status != BookingStatus.Completed &&

                    x.StartTime < (request.ExpectedEndTime ?? request.StartTime) &&
                    (x.ExpectedEndTime == null ||
                     x.ExpectedEndTime > request.StartTime),

                    cancellationToken);

            if (hasConflict)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Workspace already has a conflicting booking.");
            }

         
            booking.CustomerId = request.CustomerId;
            booking.WorkspaceId = request.WorkspaceId;
            booking.BookingDate = request.BookingDate;
            booking.StartTime = request.StartTime;
            booking.ExpectedEndTime = request.ExpectedEndTime;
            booking.NumberOfPeople = request.NumberOfPeople;
            booking.Notes = request.Notes;

            booking.UpdatedAt = DateTime.UtcNow;
            booking.UpdatedBy = _currentUserService.UserId;

        
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Booking updated successfully.");
        }
    }
}
