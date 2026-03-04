using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.GoogleCalendar.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.GoogleCalendar.Queries;

public record GetGoogleCalendarStatusQuery : IRequest<GoogleCalendarStatusDto>;

public class GetGoogleCalendarStatusQueryHandler
    : IRequestHandler<GetGoogleCalendarStatusQuery, GoogleCalendarStatusDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public GetGoogleCalendarStatusQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<GoogleCalendarStatusDto> Handle(
        GetGoogleCalendarStatusQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var syncs = await _unitOfWork.GoogleCalendarSyncs
            .GetByUserIdAsync(userId, cancellationToken);

        var sync = syncs.FirstOrDefault();

        if (sync is null)
            return new GoogleCalendarStatusDto(false, null, null, false);

        return new GoogleCalendarStatusDto(
            IsConnected: true,
            CalendarId: sync.CalendarId,
            LastSyncedAt: sync.LastSyncedAt,
            IsEnabled: sync.IsEnabled);
    }
}
