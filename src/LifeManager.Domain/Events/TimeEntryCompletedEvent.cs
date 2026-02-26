using MediatR;

namespace LifeManager.Domain.Events;

public record TimeEntryCompletedEvent(
    Guid TimeEntryId,
    Guid UserId,
    Guid TaskId,
    string TaskTitle,
    int DurationMinutes) : INotification;
