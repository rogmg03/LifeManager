using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.FreeTime.DTOs;

public record FreeTimeTransactionDto(
    Guid Id,
    string Type,
    int MinutesDelta,
    int BalanceAfterMinutes,
    string? Notes,
    Guid? TimeEntryId,
    Guid? ScheduleBlockId,
    DateTime CreatedAt)
{
    public static FreeTimeTransactionDto FromEntity(FreeTimeTransaction t) => new(
        t.Id,
        t.Type.ToString(),
        t.MinutesDelta,
        t.BalanceAfterMinutes,
        t.Notes,
        t.TimeEntryId,
        t.ScheduleBlockId,
        t.CreatedAt);
}
