namespace LifeManager.Application.Features.Workouts.DTOs;

public record ExerciseProgressDto(
    string ExerciseName,
    decimal? MaxWeight,
    decimal? AvgReps,
    decimal? Estimated1RM,
    IReadOnlyList<ExerciseHistoryPointDto> History);

public record ExerciseHistoryPointDto(
    DateTime Date,
    decimal MaxWeight,
    decimal TotalVolume,
    int BestSetReps);
