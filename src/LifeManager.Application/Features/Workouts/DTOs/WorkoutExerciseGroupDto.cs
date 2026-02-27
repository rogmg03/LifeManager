namespace LifeManager.Application.Features.Workouts.DTOs;

public record WorkoutExerciseGroupDto(
    string ExerciseName,
    Guid? RoutineItemId,
    int TargetSets,
    int TargetReps,
    decimal? TargetWeight,
    int RestSeconds,
    IReadOnlyList<WorkoutSetDto> Sets);
