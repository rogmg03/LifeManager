namespace LifeManager.Domain.Entities;

public class Routine : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? EstimatedDurationMinutes { get; set; }
    public string? Category { get; set; }
    public bool IsArchived { get; set; } = false;
    public int SortOrder { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public ICollection<RoutineItem> Items { get; set; } = new List<RoutineItem>();
    public ICollection<WorkoutSession> Sessions { get; set; } = new List<WorkoutSession>();
}
