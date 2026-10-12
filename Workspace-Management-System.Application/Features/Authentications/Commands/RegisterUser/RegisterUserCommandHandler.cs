using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
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
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Identity;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Authentications.Commands.RegisterUser
{
    public class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public RegisterUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResources> localizer)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            var existingUser =
                await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    _localizer["EmailAlreadyExists"].Value);
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var createResult = await _userManager.CreateAsync(
                user,
                request.Password);

            if (!createResult.Succeeded)
            {
             
                var errors = string.Join(
                    ", ",
                    createResult.Errors.Select(e => e.Description));

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(
                user,
                "Receptionist");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    _localizer["FailedToAssignRole"].Value);
            }

            var employee = new Employee
            {
                FirstNameEn = request.FirstNameEn.Trim(),
                FirstNameAr = request.FirstNameAr.Trim(),
                LastNameEn = request.LastNameEn.Trim(),
                LastNameAr = request.LastNameAr.Trim(),

                Email = request.Email,
                Phone = request.PhoneNumber,
                Gender = request.Gender,

                HireDate = DateTime.UtcNow,
                Status = EmployeeStatus.Active,

                UserId = user.Id,
                WorkspaceId = null,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Id
            };

            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.SaveAsync();

            return Result<string>.Success(
                _localizer["UserRegisteredSuccessfully"].Value);
        }
    }
}
