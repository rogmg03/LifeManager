using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Events;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.FreeTime.EventHandlers;

/// <summary>
/// Handles TimeEntryCompletedEvent: auto-creates a FreeTimeTransaction (Earned)
/// and writes back the EarnedTransactionId on the TimeEntry.
/// </summary>
public class TimeEntryCompletedEventHandler : INotificationHandler<TimeEntryCompletedEvent>
{
    private readonly IUnitOfWork _uow;
    private readonly IFreeTimeCalculator _calculator;

    public TimeEntryCompletedEventHandler(IUnitOfWork uow, IFreeTimeCalculator calculator)
    { _uow = uow; _calculator = calculator; }

    public async Task Handle(TimeEntryCompletedEvent notification, CancellationToken ct)
    {
        // 1. Determine ratio (default 1:1 if user has not configured one)
        var ratio = await _uow.FreeTimeRatios.GetByUserIdAsync(notification.UserId, ct);
        var workMinutesPerFree = ratio?.WorkMinutesPerFreeMinute ?? 1.0m;

        // 2. Calculate earned minutes
        var earnedMinutes = _calculator.CalculateEarnedMinutes(notification.DurationMinutes, workMinutesPerFree);
        if (earnedMinutes <= 0) return; // nothing to record

        // 3. Current running balance
        var currentBalance = await _uow.FreeTimeTransactions.GetLatestBalanceForUserAsync(notification.UserId, ct);

        // 4. Create the Earned transaction
        var transaction = new FreeTimeTransaction
        {
            UserId = notification.UserId,
            TimeEntryId = notification.TimeEntryId,
            Type = TransactionType.Earned,
            MinutesDelta = earnedMinutes,
            BalanceAfterMinutes = currentBalance + earnedMinutes,
            Notes = $"Earned from \"{notification.TaskTitle}\" ({notification.DurationMinutes} min worked)"
        };
        await _uow.FreeTimeTransactions.AddAsync(transaction, ct);

        // 5. Link back to TimeEntry
        var entry = await _uow.TimeEntries.GetByIdAsync(notification.TimeEntryId, ct);
        if (entry is not null)
        {
            entry.EarnedTransactionId = transaction.Id;
            _uow.TimeEntries.Update(entry);
        }

        await _uow.SaveChangesAsync(ct);
    }
}
