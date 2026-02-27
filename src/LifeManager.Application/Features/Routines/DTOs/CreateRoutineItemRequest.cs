namespace LifeManager.Application.Features.Routines.DTOs;

public record CreateRoutineItemRequest(
    string ExerciseName,
    string? Description,
    int TargetSets,
    int TargetReps,
    decimal? TargetWeight,
    int RestSeconds = 60,
    int SortOrder = 0);
