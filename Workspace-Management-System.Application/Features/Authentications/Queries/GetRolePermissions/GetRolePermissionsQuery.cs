using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Authentications.Queries.GetRolePermissions
{
    public class GetRolePermissionsQuery
        : IRequest<Result<GetRolePermissionsResponse>>
    {
    }
}