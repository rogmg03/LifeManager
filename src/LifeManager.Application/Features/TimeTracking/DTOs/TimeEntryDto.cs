using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.TimeTracking.DTOs;

public record TimeEntryDto(
    Guid Id,
    Guid TaskId,
    string TaskTitle,
    Guid? ProjectId,
    string? ProjectName,
    DateTime StartedAt,
    DateTime? EndedAt,
    int? DurationMinutes,
    string? Notes,
    bool IsManual,
    bool IsRunning,
    DateTime CreatedAt)
{
    public static TimeEntryDto FromEntity(TimeEntry te) => new(
        te.Id,
        te.TaskId,
        te.Task?.Title ?? string.Empty,
        te.Task?.ProjectId,
        te.Task?.Project?.Name,
        te.StartedAt,
        te.EndedAt,
        te.DurationMinutes,
        te.Notes,
        te.IsManual,
        te.EndedAt == null,
        te.CreatedAt);

    /// <summary>Build DTO when navigation properties are not loaded.</summary>
    public static TimeEntryDto FromEntityWithTask(TimeEntry te, string taskTitle, Guid projectId, string projectName) => new(
        te.Id,
        te.TaskId,
        taskTitle,
        projectId,
        projectName,
        te.StartedAt,
        te.EndedAt,
        te.DurationMinutes,
        te.Notes,
        te.IsManual,
        te.EndedAt == null,
        te.CreatedAt);
}
