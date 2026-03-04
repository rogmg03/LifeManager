using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.GoogleCalendar.Commands;

public record DisconnectGoogleCalendarCommand : IRequest<Unit>, IBaseCommand;

public class DisconnectGoogleCalendarCommandHandler
    : IRequestHandler<DisconnectGoogleCalendarCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public DisconnectGoogleCalendarCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        DisconnectGoogleCalendarCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        // Clear refresh token from User
        var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken)
            ?? throw new InvalidOperationException("User not found.");

        user.RefreshToken = null;
        _unitOfWork.Users.Update(user);

        // Delete all GoogleCalendarSync records for this user
        var syncs = await _unitOfWork.GoogleCalendarSyncs
            .GetByUserIdAsync(userId, cancellationToken);

        foreach (var sync in syncs)
            _unitOfWork.GoogleCalendarSyncs.Delete(sync);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
