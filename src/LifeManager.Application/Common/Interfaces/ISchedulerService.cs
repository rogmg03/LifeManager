using LifeManager.Application.Features.Scheduler.DTOs;

namespace LifeManager.Application.Common.Interfaces;

public interface ISchedulerService
{
    /// <summary>
    /// Returns pending tasks that have no schedule block on the given day,
    /// ordered by urgency then priority then due date.
    /// Only tasks with EstimatedMinutes > 0 are returned.
    /// </summary>
    Task<IReadOnlyList<ScheduleSuggestionDto>> GetUnscheduledTasksAsync(
        Guid userId, DateTime dayStart, DateTime dayEnd, CancellationToken ct = default);

    /// <summary>
    /// Returns top 10 pending tasks ordered by urgency, priority, and due date.
    /// </summary>
    Task<IReadOnlyList<ScheduleSuggestionDto>> GetSuggestionsAsync(
        Guid userId, CancellationToken ct = default);
}
