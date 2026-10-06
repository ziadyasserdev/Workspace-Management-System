
using MediatR;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Customers.Dtos;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Customers.Queries.GetCustomerTypes;

public class GetCustomerTypesQueryHandler
    : IRequestHandler<
        GetCustomerTypesQuery,
        Result<List<CustomerTypeDto>>>
{
    private readonly IStringLocalizer<SharedResources> _localizer;

    public GetCustomerTypesQueryHandler(
        IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer;
    }

    public Task<Result<List<CustomerTypeDto>>> Handle(
        GetCustomerTypesQuery request,
        CancellationToken cancellationToken)
    {
        var customerTypes = Enum.GetValues<CustomerType>()
            .Select(type => new CustomerTypeDto
            {
                Value = (int)type,
                Name = _localizer[
                    $"CustomerType_{type}"]
            })
            .ToList();

        return Task.FromResult(
            Result<List<CustomerTypeDto>>.Success(
                customerTypes,
                _localizer["CustomerTypesRetrievedSuccessfully"]));
    }
}
