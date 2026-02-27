namespace LifeManager.Domain.Entities;

public class WorkoutSession : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid? RoutineId { get; set; }
    public string RoutineName { get; set; } = string.Empty;
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public int? DurationSeconds { get; set; }
    public int TotalSets { get; set; }
    public int CompletedSets { get; set; } = 0;
    public decimal CompletionRate { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Routine? Routine { get; set; }
    public ICollection<WorkoutSet> Sets { get; set; } = new List<WorkoutSet>();
}
