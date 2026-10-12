using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Authentications.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Authentications.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<Result<string>>
    {
        public string FirstNameEn { get; set; } = string.Empty;
        public string FirstNameAr { get; set; } = string.Empty;

        public string LastNameEn { get; set; } = string.Empty;
        public string LastNameAr { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public Gender Gender { get; set; }

        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
