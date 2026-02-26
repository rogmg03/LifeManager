using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.RecurrenceRules.DTOs;

public record RecurrenceRuleDto(
    Guid Id,
    Guid TaskId,
    string Pattern,
    int? IntervalDays,
    string? DaysOfWeek,
    bool IsActive,
    DateOnly NextDueDate,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static RecurrenceRuleDto FromEntity(RecurrenceRule r) => new(
        r.Id, r.TaskId,
        r.Pattern.ToString(), r.IntervalDays, r.DaysOfWeek,
        r.IsActive, r.NextDueDate,
        r.CreatedAt, r.UpdatedAt);
}
