namespace LifeManager.Domain.Entities;

public class WorkoutLog : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Guid? RoutineId { get; set; }
    public DateTimeOffset LoggedAt { get; set; }
    public string? Notes { get; set; }
    public int? DurationMinutes { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
    public Routine? Routine { get; set; }
}
