using LifeManager.Domain.Enums;
using TaskStatus = LifeManager.Domain.Enums.TaskStatus;

namespace LifeManager.Domain.Entities;

public class ProjectTask : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Guid? PhaseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; set; }
    public int? EstimatedMinutes { get; set; }
    public int SortOrder { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Navigation
    public Project Project { get; set; } = null!;
    public Phase? Phase { get; set; }
    public ICollection<Subtask> Subtasks { get; set; } = new List<Subtask>();
    public RecurrenceRule? RecurrenceRule { get; set; }
}
