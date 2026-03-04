using LifeManager.Application.Features.GoogleCalendar.Commands;
using LifeManager.Application.Features.GoogleCalendar.DTOs;
using LifeManager.Application.Features.GoogleCalendar.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class GoogleCalendarController : ControllerBase
{
    private readonly IMediator _mediator;
    public GoogleCalendarController(IMediator mediator) => _mediator = mediator;

    // GET /api/google-calendar/status
    [HttpGet("api/google-calendar/status")]
    public async Task<IActionResult> GetStatus(CancellationToken ct)
        => Ok(await _mediator.Send(new GetGoogleCalendarStatusQuery(), ct));

    // POST /api/google-calendar/connect
    [HttpPost("api/google-calendar/connect")]
    public async Task<IActionResult> Connect(
        [FromBody] ConnectGoogleCalendarRequest request,
        CancellationToken ct)
    {
        await _mediator.Send(
            new ConnectGoogleCalendarCommand(request.Code, request.RedirectUri), ct);
        return Ok(new { message = "Google Calendar connected successfully." });
    }

    // POST /api/google-calendar/sync
    [HttpPost("api/google-calendar/sync")]
    public async Task<IActionResult> Sync(CancellationToken ct)
    {
        var result = await _mediator.Send(new SyncGoogleCalendarCommand(), ct);
        return Ok(result);
    }

    // DELETE /api/google-calendar/disconnect
    [HttpDelete("api/google-calendar/disconnect")]
    public async Task<IActionResult> Disconnect(CancellationToken ct)
    {
        await _mediator.Send(new DisconnectGoogleCalendarCommand(), ct);
        return Ok(new { message = "Google Calendar disconnected successfully." });
    }
}
