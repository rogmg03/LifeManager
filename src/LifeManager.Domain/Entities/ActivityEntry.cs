using LifeManager.Domain.Enums;

namespace LifeManager.Domain.Entities;

public class ActivityEntry : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid? ProjectId { get; set; }
    public ActivityType ActivityType { get; set; }
    public string Description { get; set; } = string.Empty;

    /// <summary>ID of the entity that triggered this entry (Task, Document, Phase, etc.).</summary>
    public Guid? EntityId { get; set; }

    /// <summary>Friendly type name of the triggering entity, e.g. "Task", "Document".</summary>
    public string? EntityType { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Project? Project { get; set; }
}
