using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Contracts.Services;
using Workspace_Management_System.Application.Features.Authentications.Dtos;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Identity;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Authentications.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this._userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return Result<string>.Failure(ResultStatus.Failure, "Email already exists.");
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<string>.Failure(ResultStatus.Failure, errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Receptionist");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return Result<string>.Failure(ResultStatus.Failure, "Failed to assign role.");
            }

            var employee = new Employee
            {
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Id,
                FirstName = request.FirstName,
                Gender = request.Gender,
                Phone = request.PhoneNumber,
                LastName = request.LastName,
                UserId = user.Id,
                HireDate = DateTime.UtcNow,
                Status = EmployeeStatus.Active,
                WorkspaceId = null // Assuming the workspace is not assigned at registration
            };
            await unitOfWork.Employees.AddAsync(employee);
            await unitOfWork.SaveAsync();
          return Result<string>.Success("User registered successfully.");

        }
    }
}
