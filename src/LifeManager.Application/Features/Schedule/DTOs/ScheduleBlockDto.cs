using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Schedule.DTOs;

public record ScheduleBlockDto(
    Guid Id,
    string Title,
    DateTime StartTime,
    DateTime EndTime,
    string BlockType,
    string Status,
    string? Notes,
    Guid? ProjectId,
    Guid? TaskId,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static ScheduleBlockDto FromEntity(ScheduleBlock b) => new(
        b.Id, b.Title, b.StartTime, b.EndTime,
        b.BlockType.ToString(), b.Status.ToString(),
        b.Notes, b.ProjectId, b.TaskId,
        b.CreatedAt, b.UpdatedAt);
}
