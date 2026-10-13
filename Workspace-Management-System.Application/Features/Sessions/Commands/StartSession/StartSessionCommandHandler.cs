using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Sessions.Commands.StartSession
{
    public class StartSessionCommandHandler
    : IRequestHandler<StartSessionCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public StartSessionCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(
            StartSessionCommand request,
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

                var workspace = await _unitOfWork.Workspaces
                    .Query()
                    .FirstOrDefaultAsync(
                        x => x.Id == request.WorkspaceId &&
                             !x.IsDeleted,
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

                if (workspace.Status != WorkspaceStatus.Available)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        _localizer["WorkspaceIsNotAvailable"].Value);
                }

                if (request.NumberOfPeople > workspace.Capacity)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        _localizer["NumberOfPeopleExceedsWorkspaceCapacity"].Value);
                }

                var customer = await _unitOfWork.Customers
                    .Query()
                    .FirstOrDefaultAsync(
                        x => x.Id == request.CustomerId &&
                             !x.IsDeleted,
                        cancellationToken);

                if (customer is null)
                {
                    return Result<int>.Failure(
                        ResultStatus.NotFound,
                        _localizer["CustomerNotFound"].Value);
                }

                var pricingPlan = await _unitOfWork.PricingPlans
                    .Query()
                    .FirstOrDefaultAsync(
                        x => x.Id == request.PricingPlanId &&
                             !x.IsDeleted,
                        cancellationToken);

                if (pricingPlan is null)
                {
                    return Result<int>.Failure(
                        ResultStatus.NotFound,
                        _localizer["PricingPlanNotFound"].Value);
                }

                var employee = await _unitOfWork.Employees
                    .Query()
                    .FirstOrDefaultAsync(
                        x => x.UserId == currentUserId &&
                             !x.IsDeleted,
                        cancellationToken);

                if (employee is null)
                {
                    return Result<int>.Failure(
                        ResultStatus.Forbidden,
                        _localizer["CurrentUserIsNotRegisteredAsEmployee"].Value);
                }

                if (employee.Status != EmployeeStatus.Active)
                {
                    return Result<int>.Failure(
                        ResultStatus.Forbidden,
                        _localizer["EmployeeIsNotActive"].Value);
                }

                if (employee.WorkspaceId != request.WorkspaceId)
                {
                    return Result<int>.Failure(
                        ResultStatus.Forbidden,
                        _localizer["EmployeeIsNotAssignedToThisWorkspace"].Value);
                }

                var activeSessionExists = await _unitOfWork.Sessions
                    .Query()
                    .AnyAsync(
                        x => x.WorkspaceId == request.WorkspaceId &&
                             x.Status == SessionStatus.Active &&
                             !x.IsDeleted,
                        cancellationToken);

                if (activeSessionExists)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        _localizer["WorkspaceAlreadyHasActiveSession"].Value);
                }

                Booking? booking = null;

                if (request.BookingId.HasValue)
                {
                    booking = await _unitOfWork.Bookings
                        .Query()
                        .FirstOrDefaultAsync(
                            x => x.Id == request.BookingId.Value &&
                                 !x.IsDeleted,
                            cancellationToken);

                    if (booking is null)
                    {
                        return Result<int>.Failure(
                            ResultStatus.NotFound,
                            _localizer["BookingNotFound"].Value);
                    }

                    if (booking.CustomerId != request.CustomerId)
                    {
                        return Result<int>.Failure(
                            ResultStatus.Conflict,
                            _localizer["BookingDoesNotBelongToCustomer"].Value);
                    }

                    if (booking.WorkspaceId != request.WorkspaceId)
                    {
                        return Result<int>.Failure(
                            ResultStatus.Conflict,
                            _localizer["BookingDoesNotBelongToWorkspace"].Value);
                    }

                    if (booking.Status != BookingStatus.Confirmed)
                    {
                        return Result<int>.Failure(
                            ResultStatus.Conflict,
                            _localizer["BookingCannotBeCheckedInWithStatus",
                                booking.Status].Value);
                    }

                    var bookingAlreadyUsed =
                        await _unitOfWork.Sessions
                            .Query()
                            .AnyAsync(
                                x => x.BookingId == request.BookingId.Value &&
                                     !x.IsDeleted,
                                cancellationToken);

                    if (bookingAlreadyUsed)
                    {
                        return Result<int>.Failure(
                            ResultStatus.Conflict,
                            _localizer["BookingHasAlreadyBeenUsed"].Value);
                    }
                }

                var now = DateTime.UtcNow;

                var session = new Session
                {
                    CustomerId = request.CustomerId,
                    BookingId = request.BookingId,
                    WorkspaceId = request.WorkspaceId,
                    PricingPlanId = request.PricingPlanId,
                    EmployeeId = employee.Id,
                    StartTime = now,
                    EndTime = null,
                    NumberOfPeople = request.NumberOfPeople,
                    Status = SessionStatus.Active,
                    CreatedAt = now,
                    CreatedBy = currentUserId
                };

                await _unitOfWork.Sessions.AddAsync(session);

                var history = new SessionWorkspaceHistory
                {
                    Session = session,
                    WorkspaceId = workspace.Id,
                    StartTime = now,
                    EndTime = null,
                    EmployeeId = employee.Id,
                    Reason = _localizer["SessionStarted"].Value,
                    CreatedAt = now,
                    CreatedBy = currentUserId
                };

                await _unitOfWork.SessionWorkspaceHistories
                    .AddAsync(history);

                workspace.Status = WorkspaceStatus.Occupied;
                workspace.UpdatedAt = now;
                workspace.UpdatedBy = currentUserId;

                _unitOfWork.Workspaces.Update(workspace);

                if (booking is not null)
                {
                    booking.Status = BookingStatus.CheckedIn;
                    booking.UpdatedAt = now;
                    booking.UpdatedBy = currentUserId;

                    _unitOfWork.Bookings.Update(booking);
                }

                await _unitOfWork.SaveAsync();

                await transaction.CommitAsync(cancellationToken);

                return Result<int>.Success(
                    session.Id,
                    _localizer["SessionStartedSuccessfully"].Value);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }


}