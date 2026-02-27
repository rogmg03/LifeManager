using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Events;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Schedule.EventHandlers;

/// <summary>
/// Handles FreeTimeCashedOutEvent: auto-creates a FreeTime ScheduleBlock stub
/// and writes back ScheduleBlockId on the FreeTimeTransaction.
/// </summary>
public class FreeTimeCashedOutEventHandler : INotificationHandler<FreeTimeCashedOutEvent>
{
    private readonly IUnitOfWork _uow;

    public FreeTimeCashedOutEventHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(FreeTimeCashedOutEvent notification, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        // Create the stub ScheduleBlock
        var block = new ScheduleBlock
        {
            UserId = notification.UserId,
            Title = "Free Time",
            StartTime = now,
            EndTime = now.AddMinutes(notification.MinutesSpent),
            BlockType = BlockType.FreeTime,
            Status = BlockStatus.Scheduled
        };

        await _uow.ScheduleBlocks.AddAsync(block, ct);

        // Write back ScheduleBlockId on the originating FreeTimeTransaction
        var transaction = await _uow.FreeTimeTransactions.GetByIdAsync(notification.TransactionId, ct);
        if (transaction is not null)
        {
            transaction.ScheduleBlockId = block.Id;
            _uow.FreeTimeTransactions.Update(transaction);
        }

        await _uow.SaveChangesAsync(ct);
    }
}
