namespace LifeManager.Application.Features.Dashboard.DTOs;

public class RecentActivityDto
{
    public Guid Id { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateTime OccurredAt { get; init; }
    public string? ProjectName { get; init; }
}
