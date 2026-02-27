using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.WorkoutLogs.DTOs;

public record WorkoutLogDto(
    Guid Id,
    Guid ProjectId,
    Guid? RoutineId,
    DateTimeOffset LoggedAt,
    string? Notes,
    int? DurationMinutes,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static WorkoutLogDto FromEntity(WorkoutLog w) => new(
        w.Id, w.ProjectId, w.RoutineId, w.LoggedAt,
        w.Notes, w.DurationMinutes, w.CreatedAt, w.UpdatedAt);
}
