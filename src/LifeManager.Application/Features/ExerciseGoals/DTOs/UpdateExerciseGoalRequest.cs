namespace LifeManager.Application.Features.ExerciseGoals.DTOs;

public record UpdateExerciseGoalRequest(
    string MetricName,
    decimal TargetValue,
    string Unit,
    DateOnly? Deadline);
