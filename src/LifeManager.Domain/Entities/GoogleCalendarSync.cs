namespace LifeManager.Domain.Entities;

public class GoogleCalendarSync : BaseEntity
{
    public Guid UserId { get; set; }
    public string CalendarId { get; set; } = string.Empty;
    public DateTime? LastSyncedAt { get; set; }
    public bool IsEnabled { get; set; } = true;

    // Navigation
    public User User { get; set; } = null!;
}
