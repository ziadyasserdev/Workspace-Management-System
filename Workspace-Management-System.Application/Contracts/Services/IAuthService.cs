using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Features.Authentications.Dtos;
using Workspace_Management_System.Domain.Identity;

namespace Workspace_Management_System.Application.Contracts.Services
{
    public interface IAuthService
    {
        public Task<AuthTokenDto> GenerateToken(ApplicationUser user);
        // public RefreshToken GenerateRefreshToken();
        //public Task<AuthTokenDto> RefreshTokenAsync(string token);
        //public Task<bool> RevokeTokenAsync(string token);
    }
}
