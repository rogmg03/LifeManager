using MediatR;

namespace LifeManager.Domain.Events;

public record ScheduleBlockSkippedEvent(
    Guid ScheduleBlockId,
    Guid UserId,
    string Title,
    DateTime StartTime,
    DateTime EndTime) : INotification;
