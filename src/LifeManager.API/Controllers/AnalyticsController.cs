using LifeManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsReadService _analyticsReadService;
    private readonly ICurrentUserService _currentUserService;

    public AnalyticsController(
        IAnalyticsReadService analyticsReadService,
        ICurrentUserService currentUserService)
    {
        _analyticsReadService = analyticsReadService;
        _currentUserService = currentUserService;
    }

    // GET /api/analytics/productivity?period=week
    [HttpGet("api/analytics/productivity")]
    public async Task<IActionResult> GetProductivity([FromQuery] string period = "week", CancellationToken ct = default)
        => Ok(await _analyticsReadService.GetProductivityAsync(_currentUserService.UserId, period, ct));

    // GET /api/analytics/academic?projectId=
    [HttpGet("api/analytics/academic")]
    public async Task<IActionResult> GetAcademic([FromQuery] Guid projectId, CancellationToken ct = default)
    {
        var result = await _analyticsReadService.GetAcademicAsync(_currentUserService.UserId, projectId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    // GET /api/analytics/exercise
    [HttpGet("api/analytics/exercise")]
    public async Task<IActionResult> GetExercise(CancellationToken ct = default)
        => Ok(await _analyticsReadService.GetExerciseAsync(_currentUserService.UserId, ct));

    // GET /api/analytics/project-health
    [HttpGet("api/analytics/project-health")]
    public async Task<IActionResult> GetProjectHealth(CancellationToken ct)
        => Ok(await _analyticsReadService.GetProjectHealthAsync(_currentUserService.UserId, ct));

    // GET /api/analytics/time-tracking?period=week&groupBy=project
    [HttpGet("api/analytics/time-tracking")]
    public async Task<IActionResult> GetTimeTracking(
        [FromQuery] string period = "week",
        [FromQuery] string groupBy = "project",
        CancellationToken ct = default)
        => Ok(await _analyticsReadService.GetTimeTrackingAsync(_currentUserService.UserId, period, groupBy, ct));
}
