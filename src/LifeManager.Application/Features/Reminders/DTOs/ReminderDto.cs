using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Reminders.DTOs;

public record ReminderDto(
    Guid Id,
    Guid UserId,
    Guid? TaskId,
    Guid? ScheduleBlockId,
    string Title,
    string ReminderType,
    string Status,
    DateTime RemindAt,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static ReminderDto FromEntity(Reminder r) => new(
        r.Id, r.UserId, r.TaskId, r.ScheduleBlockId,
        r.Title, r.ReminderType.ToString(), r.Status.ToString(),
        r.RemindAt, r.Notes, r.CreatedAt, r.UpdatedAt);
}
