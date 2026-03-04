using LifeManager.Domain.Enums;

namespace LifeManager.Domain.Entities;

public class DailyGoal : BaseEntity
{
    public Guid UserId { get; set; }
    public DailyGoalCategory Category { get; set; }

    /// <summary>Target minutes to spend on this category each day.</summary>
    public int GoalMinutes { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}
