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

namespace Workspace_Management_System.Application.Features.Sessions.Commands.EndSession
{
    public class EndSessionCommandHandler
     : IRequestHandler<EndSessionCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public EndSessionCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
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
                        "Session is not active.");
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
                        "Current user is not registered as an employee.");
                }

                if (employee.Status != EmployeeStatus.Active)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Forbidden,
                        "Employee is not active.");
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
                        "Workspace not found.");
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

                if (currentHistory is not null)
                {
                    currentHistory.EndTime = now;
                    currentHistory.UpdatedAt = now;
                    currentHistory.UpdatedBy = currentUserId;

                    _unitOfWork.SessionWorkspaceHistories
                        .Update(currentHistory);
                }

             
                workspace.Status = WorkspaceStatus.Available;
                workspace.UpdatedAt = now;
                workspace.UpdatedBy = currentUserId;

                _unitOfWork.Workspaces.Update(workspace);

                await _unitOfWork.SaveAsync();

                await transaction.CommitAsync(cancellationToken);

                return Result<bool>.Success(
                    true,
                    "Session ended successfully.");
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
