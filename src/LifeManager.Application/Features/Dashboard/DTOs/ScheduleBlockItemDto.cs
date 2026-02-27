namespace LifeManager.Application.Features.Dashboard.DTOs;

public class ScheduleBlockItemDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string BlockType { get; init; } = string.Empty;
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string Status { get; init; } = string.Empty;
}
