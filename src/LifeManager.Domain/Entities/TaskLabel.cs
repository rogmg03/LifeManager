namespace LifeManager.Domain.Entities;

public class TaskLabel
{
    public Guid TaskId { get; set; }
    public Guid LabelId { get; set; }

    // Navigation
    public ProjectTask Task { get; set; } = null!;
    public Label Label { get; set; } = null!;
}
