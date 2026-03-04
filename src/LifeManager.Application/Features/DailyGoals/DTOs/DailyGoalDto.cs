using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;

namespace LifeManager.Application.Features.DailyGoals.DTOs;

public record DailyGoalDto(Guid Id, DailyGoalCategory Category, int GoalMinutes)
{
    public static DailyGoalDto FromEntity(DailyGoal g) => new(g.Id, g.Category, g.GoalMinutes);
}
