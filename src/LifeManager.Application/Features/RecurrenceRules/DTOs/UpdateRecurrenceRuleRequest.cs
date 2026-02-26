using LifeManager.Domain.Enums;
namespace LifeManager.Application.Features.RecurrenceRules.DTOs;

public record UpdateRecurrenceRuleRequest(
    RecurrencePattern Pattern,
    DateOnly NextDueDate,
    int? IntervalDays,
    string? DaysOfWeek,
    bool IsActive);
