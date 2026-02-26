using LifeManager.Application.Features.TimeTracking.Commands;
using LifeManager.Application.Features.TimeTracking.DTOs;
using LifeManager.Application.Features.TimeTracking.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class TimeEntriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public TimeEntriesController(IMediator mediator) => _mediator = mediator;

    // POST /api/tasks/{taskId}/timer/start
    [HttpPost("/api/tasks/{taskId:guid}/timer/start")]
    public async Task<IActionResult> StartTimer(
        Guid taskId,
        [FromBody] StartTimerRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new StartTimerCommand(taskId, request.Notes), ct);
        return Ok(result);
    }

    // POST /api/tasks/{taskId}/timer/stop
    [HttpPost("/api/tasks/{taskId:guid}/timer/stop")]
    public async Task<IActionResult> StopTimer(Guid taskId, CancellationToken ct)
    {
        var result = await _mediator.Send(new StopTimerCommand(taskId), ct);
        return Ok(result);
    }

    // GET /api/timer/active
    [HttpGet("/api/timer/active")]
    public async Task<IActionResult> GetActiveTimer(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetActiveTimerQuery(), ct);
        return result is null ? NoContent() : Ok(result);
    }

    // POST /api/tasks/{taskId}/time-entries  (manual entry)
    [HttpPost("/api/tasks/{taskId:guid}/time-entries")]
    public async Task<IActionResult> CreateManualEntry(
        Guid taskId,
        [FromBody] CreateManualTimeEntryRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(
            new CreateManualTimeEntryCommand(taskId, request.StartedAt, request.EndedAt, request.Notes), ct);
        return Created($"/api/time-entries/{result.Id}", result);
    }

    // GET /api/tasks/{taskId}/time-entries
    [HttpGet("/api/tasks/{taskId:guid}/time-entries")]
    public async Task<IActionResult> GetTaskTimeEntries(Guid taskId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTaskTimeEntriesQuery(taskId), ct));

    // GET /api/time-entries?from=&to=
    [HttpGet("/api/time-entries")]
    public async Task<IActionResult> GetByDateRange(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken ct)
        => Ok(await _mediator.Send(new GetTimeEntriesByDateRangeQuery(from, to), ct));

    // DELETE /api/time-entries/{id}
    [HttpDelete("/api/time-entries/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteTimeEntryCommand(id), ct);
        return NoContent();
    }
}
