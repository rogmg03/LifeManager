namespace LifeManager.Domain.Entities;

/// <summary>
/// Defines how many work minutes the user must log to earn one free minute.
/// Default ratio = 1.0 means 1 min worked = 1 min free earned.
/// </summary>
public class FreeTimeRatio : BaseEntity
{
    public Guid UserId { get; set; }

    /// <summary>Work minutes required per 1 free minute earned. Default 1.0.</summary>
    public decimal WorkMinutesPerFreeMinute { get; set; } = 1.0m;

    // Navigation
    public User User { get; set; } = null!;
}
