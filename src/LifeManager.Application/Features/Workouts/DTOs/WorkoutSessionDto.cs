using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Workouts.DTOs;

public record WorkoutSessionDto(
    Guid Id,
    Guid? RoutineId,
    string RoutineName,
    DateTimeOffset StartedAt,
    DateTimeOffset? CompletedAt,
    int? DurationSeconds,
    int TotalSets,
    int CompletedSets,
    decimal CompletionRate,
    string? Notes)
{
    public static WorkoutSessionDto FromEntity(WorkoutSession s) => new(
        s.Id, s.RoutineId, s.RoutineName, s.StartedAt, s.CompletedAt,
        s.DurationSeconds, s.TotalSets, s.CompletedSets, s.CompletionRate, s.Notes);
}
