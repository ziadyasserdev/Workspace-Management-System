using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Services;
using Workspace_Management_System.Application.Features.Authentications.Dtos;
using Workspace_Management_System.Domain.Identity;

namespace Workspace_Management_System.Application.Features.Authentications.Commands.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthTokenDto>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAuthService authService;

        public LoginUserCommandHandler(UserManager<ApplicationUser> userManager, IAuthService authService)
        {
            this.userManager = userManager;
            this.authService = authService;
        }
        public async Task<Result<AuthTokenDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = new AuthTokenDto();

            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return Result<AuthTokenDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Invalid email or password.");

           
            var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordValid)
                return Result<AuthTokenDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Invalid email or password.");
            result = await authService.GenerateToken(user);

            return Result<AuthTokenDto>.Success(result);
        }
    }
}
