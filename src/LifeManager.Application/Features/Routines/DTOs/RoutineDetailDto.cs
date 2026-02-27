using LifeManager.Application.Features.Workouts.DTOs;

namespace LifeManager.Application.Features.Routines.DTOs;

public record RoutineDetailDto(
    Guid Id,
    string Name,
    string? Description,
    int? EstimatedDurationMinutes,
    string? Category,
    bool IsArchived,
    int SortOrder,
    int ItemCount,
    DateTime? LastWorkoutDate,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<RoutineItemDto> Items,
    IReadOnlyList<WorkoutSessionDto> RecentSessions);
