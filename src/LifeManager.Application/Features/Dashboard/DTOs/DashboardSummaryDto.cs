namespace LifeManager.Application.Features.Dashboard.DTOs;

public class DashboardSummaryDto
{
    public int ActiveProjectsCount { get; init; }
    public int TasksCompletedToday { get; init; }
    public int TasksDueToday { get; init; }
    public int FreeTimeBalanceMinutes { get; init; }
    public string? ActiveTimerProjectName { get; init; }
}
