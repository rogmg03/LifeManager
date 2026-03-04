using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.GoogleCalendar.Commands;

public record ConnectGoogleCalendarCommand(string Code, string RedirectUri)
    : IRequest<Unit>, IBaseCommand;

public class ConnectGoogleCalendarCommandHandler
    : IRequestHandler<ConnectGoogleCalendarCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IGoogleTokenService _googleTokenService;

    public ConnectGoogleCalendarCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IGoogleTokenService googleTokenService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _googleTokenService = googleTokenService;
    }

    public async Task<Unit> Handle(
        ConnectGoogleCalendarCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        // Exchange auth code for tokens
        var (_, refreshToken) = await _googleTokenService.ExchangeCodeAsync(
            request.Code, request.RedirectUri, cancellationToken);

        // Persist refresh token on User
        var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken)
            ?? throw new InvalidOperationException("User not found.");

        user.RefreshToken = refreshToken;
        _unitOfWork.Users.Update(user);

        // Create or update GoogleCalendarSync record
        const string primaryCalendarId = "primary";
        var existing = await _unitOfWork.GoogleCalendarSyncs
            .GetByUserAndCalendarAsync(userId, primaryCalendarId, cancellationToken);

        if (existing is null)
        {
            var sync = new GoogleCalendarSync
            {
                UserId = userId,
                CalendarId = primaryCalendarId,
                IsEnabled = true
            };
            await _unitOfWork.GoogleCalendarSyncs.AddAsync(sync, cancellationToken);
        }
        else
        {
            existing.IsEnabled = true;
            _unitOfWork.GoogleCalendarSyncs.Update(existing);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
