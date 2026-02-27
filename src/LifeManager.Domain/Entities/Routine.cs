namespace LifeManager.Domain.Entities;

public class Routine : BaseEntity
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? DayOfWeek { get; set; }
    public int SortOrder { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
}
