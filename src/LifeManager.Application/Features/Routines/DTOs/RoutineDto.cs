namespace LifeManager.Application.Features.Routines.DTOs;

public record RoutineDto(
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
    DateTime UpdatedAt);
