using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Sessions.Commands.ChangeSessionWorkspace;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

public class ChangeSessionWorkspaceCommandHandler
    : IRequestHandler<ChangeSessionWorkspaceCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public ChangeSessionWorkspaceCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _localizer = localizer;
    }

    public async Task<Result<bool>> Handle(
        ChangeSessionWorkspaceCommand request,
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
                    _localizer["OnlyActiveSessionsCanChangeWorkspace"].Value);
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
                    _localizer["NotAuthorizedToChangeSessionWorkspace"].Value);
            }

            var currentWorkspace = await _unitOfWork.Workspaces
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == session.WorkspaceId &&
                         !x.IsDeleted,
                    cancellationToken);

            if (currentWorkspace is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["CurrentWorkspaceNotFound"].Value);
            }

            if (currentWorkspace.Id == request.NewWorkspaceId)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["SessionAlreadyUsesThisWorkspace"].Value);
            }

            var newWorkspace = await _unitOfWork.Workspaces
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.NewWorkspaceId &&
                         !x.IsDeleted,
                    cancellationToken);

            if (newWorkspace is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["NewWorkspaceNotFound"].Value);
            }

            if (!newWorkspace.IsActive)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["NewWorkspaceIsInactive"].Value);
            }

            if (newWorkspace.Status != WorkspaceStatus.Available)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["NewWorkspaceIsNotAvailable"].Value);
            }

            if (session.NumberOfPeople > newWorkspace.Capacity)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["NumberOfPeopleExceedsNewWorkspaceCapacity"].Value);
            }

            var activeSessionExists = await _unitOfWork.Sessions
                .Query()
                .AnyAsync(
                    x => x.WorkspaceId == newWorkspace.Id &&
                         x.Status == SessionStatus.Active &&
                         !x.IsDeleted &&
                         x.Id != session.Id,
                    cancellationToken);

            if (activeSessionExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["NewWorkspaceAlreadyHasActiveSession"].Value);
            }

            var now = DateTime.UtcNow;

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
                    _localizer["CurrentWorkspaceHistoryNotFound"].Value);
            }

            currentHistory.EndTime = now;
            currentHistory.UpdatedAt = now;
            currentHistory.UpdatedBy = currentUserId;

            _unitOfWork.SessionWorkspaceHistories
                .Update(currentHistory);

            var newHistory = new SessionWorkspaceHistory
            {
                SessionId = session.Id,
                WorkspaceId = newWorkspace.Id,
                StartTime = now,
                EndTime = null,
                EmployeeId = currentHistory.EmployeeId,
                Reason = _localizer["SessionWorkspaceChanged"].Value,
                CreatedAt = now,
                CreatedBy = currentUserId
            };

            await _unitOfWork.SessionWorkspaceHistories
                .AddAsync(newHistory);

            session.WorkspaceId = newWorkspace.Id;
            session.UpdatedAt = now;
            session.UpdatedBy = currentUserId;

            _unitOfWork.Sessions.Update(session);

            currentWorkspace.Status = WorkspaceStatus.Available;
            currentWorkspace.UpdatedAt = now;
            currentWorkspace.UpdatedBy = currentUserId;

            _unitOfWork.Workspaces.Update(currentWorkspace);

            newWorkspace.Status = WorkspaceStatus.Occupied;
            newWorkspace.UpdatedAt = now;
            newWorkspace.UpdatedBy = currentUserId;

            _unitOfWork.Workspaces.Update(newWorkspace);

            await _unitOfWork.SaveAsync();

            await transaction.CommitAsync(cancellationToken);

            return Result<bool>.Success(
                true,
                _localizer["SessionWorkspaceChangedSuccessfully"].Value);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}