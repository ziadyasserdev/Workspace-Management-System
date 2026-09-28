using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.SessionProducts.Commands.ClearProducts
{
    public class ClearProductsHandler
        : IRequestHandler<ClearProductsCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public ClearProductsHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(
            ClearProductsCommand request,
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
                    "Products can only be cleared from active sessions.");
            }

            var sessionProducts = await _unitOfWork.SessionProducts
                .Query()
                .Where(x => x.SessionId == request.SessionId)
                .ToListAsync(cancellationToken);

            foreach (var sessionProduct in sessionProducts)
            {
                sessionProduct.IsDeleted = true;
                sessionProduct.IsDeletedBy = _currentUser.UserId;
                sessionProduct.UpdatedAt = DateTime.UtcNow;
                sessionProduct.UpdatedBy = _currentUser.UserId;

                _unitOfWork.SessionProducts.Delete(sessionProduct);
            }

            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}