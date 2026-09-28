using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.SessionProducts.Commands.RemoveProduct
{
    public class RemoveProductHandler
        : IRequestHandler<RemoveProductCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public RemoveProductHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(
            RemoveProductCommand request,
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
                    "Products can only be removed from active sessions.");
            }


            var sessionProduct = await _unitOfWork.SessionProducts
                .Query()
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

         sessionProduct.IsDeleted = true;
         sessionProduct.IsDeletedBy = _currentUser.UserId;
        sessionProduct.UpdatedAt = DateTime.UtcNow;
        sessionProduct.UpdatedBy = _currentUser.UserId;
            _unitOfWork.SessionProducts.Delete(sessionProduct);

            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }

}