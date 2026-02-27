using LifeManager.Domain.Enums;

namespace LifeManager.Domain.Entities;

public class ScheduleBlock : BaseEntity
{
    public Guid UserId { get; set; }

    public string Title { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public BlockType BlockType { get; set; }
    public BlockStatus Status { get; set; } = BlockStatus.Scheduled;

    public string? Notes { get; set; }

    // Optional links
    public Guid? ProjectId { get; set; }
    public Guid? TaskId { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Project? Project { get; set; }
    public ProjectTask? Task { get; set; }
}
