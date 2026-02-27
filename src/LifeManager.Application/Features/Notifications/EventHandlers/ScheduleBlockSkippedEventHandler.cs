using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Events;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Notifications.EventHandlers;

/// <summary>
/// Handles ScheduleBlockSkippedEvent: creates an InApp NotificationLog entry
/// recording that a block was skipped.
/// </summary>
public class ScheduleBlockSkippedEventHandler : INotificationHandler<ScheduleBlockSkippedEvent>
{
    private readonly IUnitOfWork _uow;

    public ScheduleBlockSkippedEventHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(ScheduleBlockSkippedEvent notification, CancellationToken ct)
    {
        var log = new NotificationLog
        {
            UserId = notification.UserId,
            Channel = NotificationChannel.InApp,
            Message = $"Block '{notification.Title}' was skipped",
            IsRead = false
        };

        await _uow.NotificationLogs.AddAsync(log, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
