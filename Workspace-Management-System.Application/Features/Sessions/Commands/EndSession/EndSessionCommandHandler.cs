using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Sessions.Commands.EndSession
{
    public class EndSessionCommandHandler
      : IRequestHandler<EndSessionCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public EndSessionCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            EndSessionCommand request,
            CancellationToken cancellationToken)
        {
            await using var transaction =
                await _unitOfWork.BeginTransactionAsync();

            try
            {
                var currentUserId = _currentUserService.UserId;

                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    return Result<bool>.Failure(
                        ResultStatus.Unauthorized,
                        _localizer["UserNotAuthenticated"].Value);
                }

                var session = await _unitOfWork.Sessions
                    .Query()
                    .FirstOrDefaultAsync(
                        x => x.Id == request.SessionId &&
                             !x.IsDeleted,
                        cancellationToken);

                if (session is null)
                {
                    return Result<bool>.Failure(
                        ResultStatus.NotFound,
                        _localizer["SessionNotFound"].Value);
                }

                if (session.Status != SessionStatus.Active)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Conflict,
                        _localizer["SessionIsNotActive"].Value);
                }

                var employee = await _unitOfWork.Employees
                    .Query()
                    .FirstOrDefaultAsync(
                        x => x.UserId == currentUserId &&
                             !x.IsDeleted,
                        cancellationToken);

                if (employee is null)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Forbidden,
                        _localizer["CurrentUserIsNotRegisteredAsEmployee"].Value);
                }

                if (employee.Status != EmployeeStatus.Active)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Forbidden,
                        _localizer["EmployeeIsNotActive"].Value);
                }

                if (session.EmployeeId != employee.Id)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Forbidden,
                        _localizer["NotAuthorizedToEndThisSession"].Value);
                }

                var workspace = await _unitOfWork.Workspaces
                    .Query()
                    .FirstOrDefaultAsync(
                        x => x.Id == session.WorkspaceId &&
                             !x.IsDeleted,
                        cancellationToken);

                if (workspace is null)
                {
                    return Result<bool>.Failure(
                        ResultStatus.NotFound,
                        _localizer["WorkspaceNotFound"].Value);
                }

                if (workspace.Status != WorkspaceStatus.Occupied)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Conflict,
                        _localizer["WorkspaceIsNotOccupiedByActiveSession"].Value);
                }

                Booking? booking = null;

                if (session.BookingId.HasValue)
                {
                    booking = await _unitOfWork.Bookings
                        .Query()
                        .FirstOrDefaultAsync(
                            x => x.Id == session.BookingId.Value &&
                                 !x.IsDeleted,
                            cancellationToken);

                    if (booking is null)
                    {
                        return Result<bool>.Failure(
                            ResultStatus.NotFound,
                            _localizer["BookingAssociatedWithSessionNotFound"].Value);
                    }

                    if (booking.Status != BookingStatus.CheckedIn)
                    {
                        return Result<bool>.Failure(
                            ResultStatus.Conflict,
                            _localizer["BookingIsNotCheckedIn"].Value);
                    }
                }

                var now = DateTime.UtcNow;

                session.EndTime = now;
                session.Status = SessionStatus.Completed;
                session.UpdatedAt = now;
                session.UpdatedBy = currentUserId;

                _unitOfWork.Sessions.Update(session);

                var currentHistory = await _unitOfWork
                    .SessionWorkspaceHistories
                    .Query()
                    .Where(x =>
                        x.SessionId == session.Id &&
                        !x.IsDeleted &&
                        x.EndTime == null)
                    .OrderByDescending(x => x.StartTime)
                    .FirstOrDefaultAsync(cancellationToken);

                if (currentHistory is null)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Failure,
                        _localizer["ActiveWorkspaceHistoryNotFound"].Value);
                }

                currentHistory.EndTime = now;
                currentHistory.UpdatedAt = now;
                currentHistory.UpdatedBy = currentUserId;

                _unitOfWork.SessionWorkspaceHistories.Update(currentHistory);

                workspace.Status = WorkspaceStatus.Available;
                workspace.UpdatedAt = now;
                workspace.UpdatedBy = currentUserId;

                _unitOfWork.Workspaces.Update(workspace);

                if (booking is not null)
                {
                    booking.Status = BookingStatus.Completed;
                    booking.UpdatedAt = now;
                    booking.UpdatedBy = currentUserId;

                    _unitOfWork.Bookings.Update(booking);
                }

                await _unitOfWork.SaveAsync();

                await transaction.CommitAsync(cancellationToken);

                return Result<bool>.Success(
                    true,
                    _localizer["SessionEndedSuccessfully"].Value);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}