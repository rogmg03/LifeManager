namespace LifeManager.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string GoogleId { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }

    // Navigation
    public UserSettings? Settings { get; set; }
}
