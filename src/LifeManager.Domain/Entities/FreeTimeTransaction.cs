using LifeManager.Domain.Enums;

namespace LifeManager.Domain.Entities;

public class FreeTimeTransaction : BaseEntity
{
    public Guid UserId { get; set; }

    // Earned — linked to the time entry that generated it
    public Guid? TimeEntryId { get; set; }

    // Spent — linked to a ScheduleBlock (wired in Cycle 11)
    public Guid? ScheduleBlockId { get; set; }

    public TransactionType Type { get; set; }

    /// <summary>Positive for Earned, negative for Spent.</summary>
    public int MinutesDelta { get; set; }

    /// <summary>Running balance at the moment this transaction was recorded.</summary>
    public int BalanceAfterMinutes { get; set; }

    public string? Notes { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public TimeEntry? TimeEntry { get; set; }
    public ScheduleBlock? ScheduleBlock { get; set; }
}
