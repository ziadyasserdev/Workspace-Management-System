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

namespace Workspace_Management_System.Application.Features.Bookings.Commands.DeleteBooking
{

    public class DeleteBookingCommandHandler
        : IRequestHandler<DeleteBookingCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public DeleteBookingCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
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

            if (booking.Status == BookingStatus.CheckedIn)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["CheckedInBookingCannotBeDeleted"].Value);
            }

            if (booking.Status == BookingStatus.Completed)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["CompletedBookingCannotBeDeleted"].Value);
            }

            var hasSession = await _unitOfWork.Sessions
                .Query()
                .AnyAsync(
                    x => x.BookingId == booking.Id && !x.IsDeleted,
                    cancellationToken);

            if (hasSession)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["BookingLinkedToSessionCannotBeDeleted"].Value);
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
                _localizer["BookingDeletedSuccessfully"].Value);
        }
    }
}
