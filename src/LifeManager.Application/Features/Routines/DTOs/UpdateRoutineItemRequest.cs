namespace LifeManager.Application.Features.Routines.DTOs;

public record UpdateRoutineItemRequest(
    string ExerciseName,
    string? Description,
    int TargetSets,
    int TargetReps,
    decimal? TargetWeight,
    int RestSeconds,
    int SortOrder);
