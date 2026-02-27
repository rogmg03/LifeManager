using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Routines.DTOs;

public record RoutineItemDto(
    Guid Id,
    Guid RoutineId,
    string ExerciseName,
    string? Description,
    int TargetSets,
    int TargetReps,
    decimal? TargetWeight,
    int RestSeconds,
    int SortOrder)
{
    public static RoutineItemDto FromEntity(RoutineItem item) => new(
        item.Id, item.RoutineId, item.ExerciseName, item.Description,
        item.TargetSets, item.TargetReps, item.TargetWeight,
        item.RestSeconds, item.SortOrder);
}
