using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Workouts.DTOs;

public record WorkoutSetDto(
    Guid Id,
    string ExerciseName,
    int SetNumber,
    int TargetReps,
    decimal? TargetWeight,
    int? ActualReps,
    decimal? ActualWeight,
    bool IsCompleted,
    DateTimeOffset? CompletedAt)
{
    public static WorkoutSetDto FromEntity(WorkoutSet s) => new(
        s.Id, s.ExerciseName, s.SetNumber, s.TargetReps, s.TargetWeight,
        s.ActualReps, s.ActualWeight, s.IsCompleted, s.CompletedAt);
}
