using LifeManager.Domain.Enums;

namespace LifeManager.Domain.Entities;

public class NotificationLog : BaseEntity
{
    public Guid UserId { get; set; }
    public NotificationChannel Channel { get; set; } = NotificationChannel.InApp;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;

    // Navigation
    public User User { get; set; } = null!;
}
