namespace LifeManager.Domain.Entities;

// Not a BaseEntity — no audit timestamps per design spec
public class Subtask
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TaskId { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public int SortOrder { get; set; }

    // Navigation
    public ProjectTask Task { get; set; } = null!;
}
