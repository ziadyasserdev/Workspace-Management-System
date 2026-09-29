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

namespace Workspace_Management_System.Application.Features.Sessions.Commands.StartSession
{
    public class StartSessionCommandHandler
     : IRequestHandler<StartSessionCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public StartSessionCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            StartSessionCommand request,
            CancellationToken cancellationToken)
        {
            await using var transaction =
                await _unitOfWork.BeginTransactionAsync();

            try
            {
              
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
                        "Workspace not found.");
                }

              
                if (!workspace.IsActive)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        "Workspace is inactive.");
                }

              
                if (workspace.Status != WorkspaceStatus.Available)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        "Workspace is not available.");
                }

               
                if (request.NumberOfPeople > workspace.Capacity)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        "Number of people exceeds workspace capacity.");
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
                        "Customer not found.");
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
                        "Pricing plan not found.");
                }

              
                var currentUserId = _currentUserService.UserId;

                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    return Result<int>.Failure(
                        ResultStatus.Unauthorized,
                        "User is not authenticated.");
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
                        "Current user is not registered as an employee.");
                }

                
                if (employee.Status != EmployeeStatus.Active)
                {
                    return Result<int>.Failure(
                        ResultStatus.Forbidden,
                        "Employee is not active.");
                }

            
                if (employee.WorkspaceId != request.WorkspaceId)
                {
                    return Result<int>.Failure(
                        ResultStatus.Forbidden,
                        "Employee is not assigned to this workspace.");
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
                        "Workspace already has an active session.");
                }

                // 11. Booking validation
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
                            "Booking not found.");
                    }

                    if (booking.CustomerId != request.CustomerId)
                    {
                        return Result<int>.Failure(
                            ResultStatus.Conflict,
                            "Booking does not belong to this customer.");
                    }

                    if (booking.WorkspaceId != request.WorkspaceId)
                    {
                        return Result<int>.Failure(
                            ResultStatus.Conflict,
                            "Booking does not belong to this workspace.");
                    }

                    var bookingAlreadyUsed = await _unitOfWork.Sessions
                        .Query()
                        .AnyAsync(
                            x => x.BookingId == request.BookingId.Value &&
                                 !x.IsDeleted,
                            cancellationToken);

                    if (bookingAlreadyUsed)
                    {
                        return Result<int>.Failure(
                            ResultStatus.Conflict,
                            "This booking has already been used.");
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

                    EmployeeId = employee.Id,

                    CreatedAt = now,
                    CreatedBy = currentUserId,

                    Reason = "Session started."
                };

                await _unitOfWork.SessionWorkspaceHistories
                    .AddAsync(history);

               
                workspace.Status = WorkspaceStatus.Occupied;
                workspace.UpdatedAt = now;
                workspace.UpdatedBy = currentUserId;

                _unitOfWork.Workspaces.Update(workspace);

           
                await _unitOfWork.SaveAsync();

              
                await transaction.CommitAsync(cancellationToken);

                return Result<int>.Success(
                    session.Id,
                    "Session started successfully.");
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
