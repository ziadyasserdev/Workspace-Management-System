using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.SessionProducts.Dtos;

namespace Workspace_Management_System.Application.Features.SessionProducts.Queries.GetSessionProducts
{
    public class GetSessionProductsHandler
        : IRequestHandler<
            GetSessionProductsQuery,
            List<SessionProductResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSessionProductsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SessionProductResponseDto>> Handle(
            GetSessionProductsQuery request,
            CancellationToken cancellationToken)
        {

            var sessionProducts = await _unitOfWork.SessionProducts
                .Query()
                .Include(x => x.Product)
                .Where(x => x.SessionId == request.SessionId)
                .ToListAsync(cancellationToken);

            return sessionProducts
                .Select(x => new SessionProductResponseDto
                {
                    Id = x.Id,
                    SessionId = x.SessionId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.EnglishName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    Total = x.Quantity * x.UnitPrice
                })
                .ToList();
        }
    }
}