using LifeManager.Application.Features.FreeTimeRatios.Commands;
using LifeManager.Application.Features.FreeTimeRatios.DTOs;
using LifeManager.Application.Features.FreeTimeRatios.Queries;
using LifeManager.Application.Features.Settings.Commands;
using LifeManager.Application.Features.Settings.DTOs;
using LifeManager.Application.Features.Settings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly IMediator _mediator;
    public SettingsController(IMediator mediator) => _mediator = mediator;

    // GET /api/settings
    [HttpGet("api/settings")]
    public async Task<IActionResult> Get(CancellationToken ct)
        => Ok(await _mediator.Send(new GetUserSettingsQuery(), ct));

    // PUT /api/settings
    [HttpPut("api/settings")]
    public async Task<IActionResult> Update(
        [FromBody] UpdateUserSettingsRequest request,
        CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateUserSettingsCommand(
            request.Theme,
            request.TimeZone,
            request.DailyWorkGoalMinutes,
            request.FreeTimeRatioPercent), ct));

    // GET /api/settings/free-time-ratios
    [HttpGet("api/settings/free-time-ratios")]
    public async Task<IActionResult> GetFreeTimeRatio(CancellationToken ct)
        => Ok(await _mediator.Send(new GetFreeTimeRatioQuery(), ct));

    // PUT /api/settings/free-time-ratios
    [HttpPut("api/settings/free-time-ratios")]
    public async Task<IActionResult> UpdateFreeTimeRatio(
        [FromBody] UpdateFreeTimeRatioRequest request,
        CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateFreeTimeRatioCommand(request.WorkMinutesPerFreeMinute), ct));
}
