namespace LifeManager.Domain.Entities;

public class RoutineItem : BaseEntity
{
    public Guid RoutineId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int TargetSets { get; set; }
    public int TargetReps { get; set; }
    public decimal? TargetWeight { get; set; }
    public int RestSeconds { get; set; } = 60;
    public int SortOrder { get; set; }

    // Navigation
    public Routine Routine { get; set; } = null!;
}
