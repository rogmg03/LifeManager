namespace LifeManager.Domain.Entities;

public class WorkoutSet : BaseEntity
{
    public Guid SessionId { get; set; }
    public Guid? RoutineItemId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public int SetNumber { get; set; }
    public int TargetReps { get; set; }
    public decimal? TargetWeight { get; set; }
    public int? ActualReps { get; set; }
    public decimal? ActualWeight { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTimeOffset? CompletedAt { get; set; }

    // Navigation
    public WorkoutSession Session { get; set; } = null!;
    public RoutineItem? RoutineItem { get; set; }
}
