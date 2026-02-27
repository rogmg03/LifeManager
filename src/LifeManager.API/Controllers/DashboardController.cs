using LifeManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardReadService _dashboardReadService;
    private readonly ICurrentUserService _currentUserService;

    public DashboardController(
        IDashboardReadService dashboardReadService,
        ICurrentUserService currentUserService)
    {
        _dashboardReadService = dashboardReadService;
        _currentUserService = currentUserService;
    }

    // GET /api/dashboard/summary
    [HttpGet("api/dashboard/summary")]
    public async Task<IActionResult> GetSummary(CancellationToken ct)
        => Ok(await _dashboardReadService.GetSummaryAsync(_currentUserService.UserId, ct));

    // GET /api/dashboard/todays-schedule
    [HttpGet("api/dashboard/todays-schedule")]
    public async Task<IActionResult> GetTodaysSchedule(CancellationToken ct)
        => Ok(await _dashboardReadService.GetTodaysScheduleAsync(_currentUserService.UserId, ct));

    // GET /api/dashboard/upcoming-tasks
    [HttpGet("api/dashboard/upcoming-tasks")]
    public async Task<IActionResult> GetUpcomingTasks(CancellationToken ct)
        => Ok(await _dashboardReadService.GetUpcomingTasksAsync(_currentUserService.UserId, ct));

    // GET /api/dashboard/recent-activity
    [HttpGet("api/dashboard/recent-activity")]
    public async Task<IActionResult> GetRecentActivity(CancellationToken ct)
        => Ok(await _dashboardReadService.GetRecentActivityAsync(_currentUserService.UserId, ct));
}
