using LifeManager.Domain.Enums;
namespace LifeManager.Domain.Entities;

public class Project : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid? ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ProjectType Type { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;
    public string? Color { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Client? Client { get; set; }
}
