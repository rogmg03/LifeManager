using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.ExerciseGoals.DTOs;

public record ExerciseGoalDto(
    Guid Id,
    Guid ProjectId,
    string MetricName,
    decimal TargetValue,
    string Unit,
    DateOnly? Deadline,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static ExerciseGoalDto FromEntity(ExerciseGoal g) => new(
        g.Id, g.ProjectId, g.MetricName, g.TargetValue, g.Unit, g.Deadline, g.CreatedAt, g.UpdatedAt);
}
