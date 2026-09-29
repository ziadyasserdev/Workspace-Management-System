using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Bookings.Commands.ChangeBookingStatus;
using Workspace_Management_System.Domain.Enums;

public class ChangeBookingStatusCommandHandler
    : IRequestHandler<
        ChangeBookingStatusCommand,
        Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ChangeBookingStatusCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<bool>> Handle(
        ChangeBookingStatusCommand request,
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

      
        if (booking.Status == request.Status)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                $"Booking is already {request.Status}.");
        }

     
        if (!IsValidTransition(
                booking.Status,
                request.Status))
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                $"Cannot change booking status from " +
                $"{booking.Status} to {request.Status}.");
        }

     
        if (request.Status == BookingStatus.Confirmed)
        {
            if (booking.StartTime <= DateTime.UtcNow)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "A booking that has already started cannot be confirmed.");
            }
        }

        if (request.Status == BookingStatus.CheckedIn)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                "Booking check-in must be performed by starting a session.");
        }

        if (request.Status == BookingStatus.Completed)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                "Booking completion must be performed when its session ends.");
        }

        if (request.Status == BookingStatus.NoShow)
        {
            if (DateTime.UtcNow < booking.StartTime)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "A booking cannot be marked as no-show before its start time.");
            }
        }

     
        booking.Status = request.Status;

        booking.UpdatedAt = DateTime.UtcNow;
        booking.UpdatedBy = _currentUserService.UserId;

    
        await _unitOfWork.SaveAsync();

        return Result<bool>.Success(
            true,
            $"Booking status changed to {request.Status} successfully.");
    }

    private static bool IsValidTransition(
        BookingStatus current,
        BookingStatus next)
    {
        return current switch
        {
            BookingStatus.Pending =>
                next == BookingStatus.Confirmed ||
                next == BookingStatus.Cancelled,

            BookingStatus.Confirmed =>
                next == BookingStatus.CheckedIn ||
                next == BookingStatus.Cancelled ||
                next == BookingStatus.NoShow,

            BookingStatus.CheckedIn =>
                next == BookingStatus.Completed,

            BookingStatus.Completed => false,

            BookingStatus.Cancelled => false,

            BookingStatus.NoShow => false,

            _ => false
        };
    }
}