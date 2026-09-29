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
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Sessions.Commands.ChangeSessionWorkspace
{
    public class ChangeSessionWorkspaceCommandHandler
    : IRequestHandler<ChangeSessionWorkspaceCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public ChangeSessionWorkspaceCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
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
                        "User is not authenticated.");
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
                        "Session not found.");
                }

                if (session.Status != SessionStatus.Active)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Conflict,
                        "Only active sessions can change workspace.");
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
                        "Current workspace not found.");
                }

              
                if (currentWorkspace.Id == request.NewWorkspaceId)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Conflict,
                        "Session is already using this workspace.");
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
                        "New workspace not found.");
                }

               
                if (!newWorkspace.IsActive)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Conflict,
                        "New workspace is inactive.");
                }

                if (newWorkspace.Status != WorkspaceStatus.Available)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Conflict,
                        "New workspace is not available.");
                }

               
                if (session.NumberOfPeople > newWorkspace.Capacity)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Conflict,
                        "Number of people exceeds new workspace capacity.");
                }

               
                var activeSessionExists = await _unitOfWork.Sessions
                    .Query()
                    .AnyAsync(
                        x => x.WorkspaceId == request.NewWorkspaceId &&
                             x.Status == SessionStatus.Active &&
                             !x.IsDeleted &&
                             x.Id != session.Id,
                        cancellationToken);

                if (activeSessionExists)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Conflict,
                        "New workspace already has an active session.");
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
                        "Current workspace history was not found.");
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
                    Reason = "Session workspace changed.",
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
                    "Session workspace changed successfully.");
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
