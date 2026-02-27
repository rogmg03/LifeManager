namespace LifeManager.Application.Features.Dashboard.DTOs;

public class UpcomingTaskDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string ProjectName { get; init; } = string.Empty;
    public DateTime? DueDate { get; init; }
    public string Priority { get; init; } = string.Empty;
}
