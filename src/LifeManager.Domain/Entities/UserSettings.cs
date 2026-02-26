using LifeManager.Domain.Enums;

namespace LifeManager.Domain.Entities;

public class UserSettings : BaseEntity
{
    public Guid UserId { get; set; }
    public Theme Theme { get; set; } = Theme.Light;
    public string TimeZone { get; set; } = "UTC";
    public int DailyWorkGoalMinutes { get; set; } = 480; // 8 hours
    public int FreeTimeRatioPercent { get; set; } = 20;  // 20% of worked time = free time

    // Navigation
    public User User { get; set; } = null!;
}
