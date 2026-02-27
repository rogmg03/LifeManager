namespace LifeManager.Domain.Entities;

public class ExerciseGoal : BaseEntity
{
    public Guid ProjectId { get; set; }
    public string MetricName { get; set; } = string.Empty;
    public decimal TargetValue { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateOnly? Deadline { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
}
