using LifeManager.Domain.Enums;
namespace LifeManager.Domain.Entities;

public class Client : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public ClientStatus Status { get; set; } = ClientStatus.Active;
    public string? Notes { get; set; }
    public string Color { get; set; } = "#6366F1";

    // Navigation
    public User User { get; set; } = null!;
}
