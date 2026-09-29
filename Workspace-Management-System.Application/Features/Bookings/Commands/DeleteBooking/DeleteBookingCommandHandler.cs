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

namespace Workspace_Management_System.Application.Features.Bookings.Commands.DeleteBooking
{
    public class DeleteBookingCommandHandler
     : IRequestHandler<DeleteBookingCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteBookingCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(
            DeleteBookingCommand request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

          
            if (string.IsNullOrWhiteSpace(currentUserId))
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

            if (booking is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Booking not found.");
            }

          
            if (booking.Status == BookingStatus.CheckedIn)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Checked-in bookings cannot be deleted.");
            }

           
            if (booking.Status == BookingStatus.Completed)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Completed bookings cannot be deleted.");
            }

         
            var hasSession = await _unitOfWork.Sessions
                .Query()
                .AnyAsync(
                    x => x.BookingId == booking.Id &&
                         !x.IsDeleted,
                    cancellationToken);

            if (hasSession)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Booking cannot be deleted because it is linked to a session.");
            }

         
            var now = DateTime.UtcNow;

            booking.IsDeleted = true;
            booking.IsDeletedBy = currentUserId;
            booking.UpdatedAt = now;
            booking.UpdatedBy = currentUserId;

            _unitOfWork.Bookings.Update(booking);

          
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Booking deleted successfully.");
        }
    }
}
