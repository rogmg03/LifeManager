using LifeManager.Domain.Enums;
namespace LifeManager.Domain.Entities;

public class RecurrenceRule : BaseEntity
{
    public Guid TaskId { get; set; }
    public RecurrencePattern Pattern { get; set; }
    public int? IntervalDays { get; set; }
    public string? DaysOfWeek { get; set; }  // e.g. "Mon,Wed,Fri"
    public bool IsActive { get; set; } = true;
    public DateOnly NextDueDate { get; set; }

    // Navigation
    public ProjectTask Task { get; set; } = null!;
}
