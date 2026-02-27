using LifeManager.Domain.Enums;

namespace LifeManager.Domain.Entities;

public class Reminder : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid? TaskId { get; set; }
    public Guid? ScheduleBlockId { get; set; }

    public string Title { get; set; } = string.Empty;
    public ReminderType ReminderType { get; set; } = ReminderType.Custom;
    public ReminderStatus Status { get; set; } = ReminderStatus.Pending;
    public DateTime RemindAt { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public ProjectTask? Task { get; set; }
    public ScheduleBlock? ScheduleBlock { get; set; }
}
