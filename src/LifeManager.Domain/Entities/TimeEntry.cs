namespace LifeManager.Domain.Entities;

public class TimeEntry : BaseEntity
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public int? DurationMinutes { get; set; }
    public string? Notes { get; set; }
    public bool IsManual { get; set; }

    // Navigation
    public ProjectTask Task { get; set; } = null!;
    public User User { get; set; } = null!;
}
