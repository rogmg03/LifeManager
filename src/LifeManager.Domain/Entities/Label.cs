namespace LifeManager.Domain.Entities;

public class Label : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#6366F1";

    // Navigation
    public User User { get; set; } = null!;
}
