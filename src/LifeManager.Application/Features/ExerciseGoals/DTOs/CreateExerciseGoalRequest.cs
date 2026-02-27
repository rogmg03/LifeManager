namespace LifeManager.Application.Features.ExerciseGoals.DTOs;

public record CreateExerciseGoalRequest(
    string MetricName,
    decimal TargetValue,
    string Unit,
    DateOnly? Deadline);
