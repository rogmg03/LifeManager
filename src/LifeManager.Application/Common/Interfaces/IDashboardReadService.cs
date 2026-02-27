using LifeManager.Application.Features.Dashboard.DTOs;

namespace LifeManager.Application.Common.Interfaces;

public interface IDashboardReadService
{
    Task<DashboardSummaryDto> GetSummaryAsync(Guid userId, CancellationToken ct = default);
    Task<IEnumerable<ScheduleBlockItemDto>> GetTodaysScheduleAsync(Guid userId, CancellationToken ct = default);
    Task<IEnumerable<UpcomingTaskDto>> GetUpcomingTasksAsync(Guid userId, CancellationToken ct = default);
    Task<IEnumerable<RecentActivityDto>> GetRecentActivityAsync(Guid userId, CancellationToken ct = default);
}
