using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Workouts.DTOs;

public record WorkoutSessionDetailDto(
    Guid Id,
    Guid? RoutineId,
    string RoutineName,
    DateTimeOffset StartedAt,
    DateTimeOffset? CompletedAt,
    int? DurationSeconds,
    int TotalSets,
    int CompletedSets,
    decimal CompletionRate,
    string? Notes,
    IReadOnlyList<WorkoutExerciseGroupDto> Exercises);
