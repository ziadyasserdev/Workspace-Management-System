using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Services.Dtos;

namespace Workspace_Management_System.Application.Features.Services.Queries.GetServiceById
{
    public class GetServiceByIdQueryHandler
        : IRequestHandler<GetServiceByIdQuery, Result<ServiceResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetServiceByIdQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ServiceResponseDto>> Handle(
            GetServiceByIdQuery request,
            CancellationToken cancellationToken)
        {
            var service = await _unitOfWork.Services
                .Query()
                .AsNoTracking()
                .Where(x => x.Id == request.Id && !x.IsDeleted)
                .Select(x => new ServiceResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (service == null)
            {
                return Result<ServiceResponseDto>.Failure(
                    ResultStatus.NotFound,
                    "Service not found.");
            }

            return Result<ServiceResponseDto>.Success(
                service,
                "Service retrieved successfully.");
        }
    }
}
