using LifeManager.Domain.Enums;
namespace LifeManager.Application.Features.RecurrenceRules.DTOs;

public record CreateRecurrenceRuleRequest(
    RecurrencePattern Pattern,
    DateOnly NextDueDate,
    int? IntervalDays,
    string? DaysOfWeek);
