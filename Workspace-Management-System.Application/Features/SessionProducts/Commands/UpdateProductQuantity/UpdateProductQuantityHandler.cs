using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.SessionProducts.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.SessionProducts.Commands.UpdateProductQuantity
{
    public class UpdateProductQuantityHandler
        : IRequestHandler<
            UpdateProductQuantityCommand,
            SessionProductResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateProductQuantityHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<SessionProductResponseDto> Handle(
            UpdateProductQuantityCommand request,
            CancellationToken cancellationToken)
        {
            var session = await _unitOfWork.Sessions
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.SessionId,
                    cancellationToken);

            if (session is null)
            {
                throw new KeyNotFoundException(
                    $"Session with ID {request.SessionId} was not found.");
            }

            if (session.Status != SessionStatus.Active)
            {
                throw new InvalidOperationException(
                    "Product quantity can only be updated for active sessions.");
            }

            var sessionProduct = await _unitOfWork.SessionProducts
                .Query()
                .Include(x => x.Product)
                .FirstOrDefaultAsync(
                    x =>
                        x.SessionId == request.SessionId &&
                        x.ProductId == request.ProductId,
                    cancellationToken);

            if (sessionProduct is null)
            {
                throw new KeyNotFoundException(
                    $"Product with ID {request.ProductId} was not found in this session.");
            }

            sessionProduct.Quantity = request.Quantity;
            sessionProduct.UpdatedAt = DateTime.UtcNow;
            sessionProduct.UpdatedBy = _currentUser.UserId;
            _unitOfWork.SessionProducts.Update(sessionProduct);

            await _unitOfWork.SaveAsync();

            return new SessionProductResponseDto
            {
                Id = sessionProduct.Id,
                SessionId = sessionProduct.SessionId,
                ProductId = sessionProduct.ProductId,
                ProductName = sessionProduct.Product.EnglishName,
                Quantity = sessionProduct.Quantity,
                UnitPrice = sessionProduct.UnitPrice,
                Total = sessionProduct.Quantity * sessionProduct.UnitPrice
            };
        }
    }
}