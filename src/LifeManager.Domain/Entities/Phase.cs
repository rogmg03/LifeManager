using LifeManager.Domain.Enums;
namespace LifeManager.Domain.Entities;

public class Phase : BaseEntity
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public Priority? Priority { get; set; }
    public PhaseStatus Status { get; set; } = PhaseStatus.NotStarted;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
}
