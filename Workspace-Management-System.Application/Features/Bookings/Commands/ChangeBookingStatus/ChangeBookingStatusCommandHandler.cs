using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Bookings.Commands.ChangeBookingStatus;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;


public class ChangeBookingStatusCommandHandler
       : IRequestHandler<ChangeBookingStatusCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public ChangeBookingStatusCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _localizer = localizer;
    }

    public async Task<Result<bool>> Handle(
        ChangeBookingStatusCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return Result<bool>.Failure(
                ResultStatus.Unauthorized,
                _localizer["UserNotAuthenticated"].Value);
        }

        var booking = await _unitOfWork.Bookings
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (booking is null)
        {
            return Result<bool>.Failure(
                ResultStatus.NotFound,
                _localizer["BookingNotFound"].Value);
        }

        if (booking.Status == request.Status)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["BookingAlreadyHasThisStatus"].Value);
        }

        if (!IsValidTransition(booking.Status, request.Status))
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["InvalidBookingStatusTransition"].Value);
        }

        if (request.Status == BookingStatus.Confirmed &&
            booking.StartTime <= DateTime.UtcNow)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["BookingCannotConfirmAfterStart"].Value);
        }

        if (request.Status == BookingStatus.CheckedIn)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["BookingCheckInRequiresStartingSession"].Value);
        }

        if (request.Status == BookingStatus.Completed)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["BookingCompletionRequiresEndingSession"].Value);
        }

        if (request.Status == BookingStatus.NoShow &&
            DateTime.UtcNow < booking.StartTime)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["BookingCannotBeNoShowBeforeStart"].Value);
        }

        booking.Status = request.Status;
        booking.UpdatedAt = DateTime.UtcNow;
        booking.UpdatedBy = _currentUserService.UserId;

        await _unitOfWork.SaveAsync();

        return Result<bool>.Success(
            true,
            _localizer["BookingStatusChangedSuccessfully"].Value);
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