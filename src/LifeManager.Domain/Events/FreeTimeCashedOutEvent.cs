using MediatR;

namespace LifeManager.Domain.Events;

public record FreeTimeCashedOutEvent(
    Guid TransactionId,
    Guid UserId,
    int MinutesSpent,
    int BalanceAfterMinutes) : INotification;
