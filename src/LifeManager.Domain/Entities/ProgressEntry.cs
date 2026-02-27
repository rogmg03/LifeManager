namespace LifeManager.Domain.Entities;

public class ProgressEntry : BaseEntity
{
    public Guid GoalId { get; set; }
    public DateTimeOffset RecordedAt { get; set; }
    public decimal Value { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public ExerciseGoal Goal { get; set; } = null!;
}
