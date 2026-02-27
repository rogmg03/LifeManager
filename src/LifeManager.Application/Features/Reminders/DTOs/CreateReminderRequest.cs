using LifeManager.Domain.Enums;

namespace LifeManager.Application.Features.Reminders.DTOs;

public record CreateReminderRequest(
    string Title,
    ReminderType ReminderType,
    DateTime RemindAt,
    Guid? TaskId = null,
    Guid? ScheduleBlockId = null,
    string? Notes = null);
