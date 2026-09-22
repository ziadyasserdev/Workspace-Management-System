using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Authentications.Dtos;

namespace Workspace_Management_System.Application.Features.Authentications.Commands.Login
{
    public class LoginUserCommand : IRequest<Result<AuthTokenDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public LoginUserCommand(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }
    }
}
