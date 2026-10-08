
using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Authentications.Queries.GetCurrentUserPermissions;

public class GetCurrentUserPermissionsQuery
    : IRequest<Result<GetCurrentUserPermissionsResponse>>
{
}
