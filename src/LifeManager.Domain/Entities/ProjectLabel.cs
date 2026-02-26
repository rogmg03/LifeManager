namespace LifeManager.Domain.Entities;

public class ProjectLabel
{
    public Guid ProjectId { get; set; }
    public Guid LabelId { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
    public Label Label { get; set; } = null!;
}
