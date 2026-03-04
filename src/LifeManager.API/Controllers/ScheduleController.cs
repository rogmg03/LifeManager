using LifeManager.Application.Features.Schedule.Commands;
using LifeManager.Application.Features.Schedule.DTOs;
using LifeManager.Application.Features.Schedule.Queries;
using LifeManager.Application.Features.Scheduler.Commands;
using LifeManager.Application.Features.Scheduler.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Route("api/schedule")]
[Authorize]
public class ScheduleController : ControllerBase
{
    private readonly IMediator _mediator;
    public ScheduleController(IMediator mediator) => _mediator = mediator;

    // GET /api/schedule/daily?date=2026-02-26
    [HttpGet("daily")]
    public async Task<IActionResult> GetDaily([FromQuery] DateOnly date, CancellationToken ct)
        => Ok(await _mediator.Send(new GetDailyScheduleQuery(date), ct));

    // GET /api/schedule/weekly?startDate=2026-02-24
    [HttpGet("weekly")]
    public async Task<IActionResult> GetWeekly([FromQuery] DateOnly startDate, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWeeklyScheduleQuery(startDate), ct));

    // GET /api/schedule/monthly?month=2&year=2026
    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthly([FromQuery] int month, [FromQuery] int year, CancellationToken ct)
        => Ok(await _mediator.Send(new GetMonthlyScheduleQuery(year, month), ct));

    // POST /api/schedule/blocks
    [HttpPost("blocks")]
    public async Task<IActionResult> Create([FromBody] CreateScheduleBlockRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateScheduleBlockCommand(
            request.Title, request.StartTime, request.EndTime,
            request.BlockType, request.Notes, request.ProjectId, request.TaskId), ct);
        return CreatedAtAction(nameof(GetBlock), new { id = result.Id }, result);
    }

    // GET /api/schedule/blocks/{id} — routing anchor for CreatedAtAction; not in spec
    [HttpGet("blocks/{id:guid}")]
    public IActionResult GetBlock(Guid id) => NotFound();

    // PUT /api/schedule/blocks/{id}
    [HttpPut("blocks/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateScheduleBlockRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateScheduleBlockCommand(
            id, request.Title, request.Notes, request.ProjectId, request.TaskId), ct));

    // PUT /api/schedule/blocks/{id}/move
    [HttpPut("blocks/{id:guid}/move")]
    public async Task<IActionResult> Move(Guid id, [FromBody] MoveScheduleBlockRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new MoveScheduleBlockCommand(id, request.StartTime, request.EndTime), ct));

    // PUT /api/schedule/blocks/{id}/status
    [HttpPut("blocks/{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateScheduleBlockStatusRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateScheduleBlockStatusCommand(id, request.Status), ct));

    // DELETE /api/schedule/blocks/{id}
    [HttpDelete("blocks/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteScheduleBlockCommand(id), ct);
        return NoContent();
    }

    // POST /api/schedule/auto-fill?date=2026-03-03
    [HttpPost("auto-fill")]
    public async Task<IActionResult> AutoFill([FromQuery] DateOnly date, CancellationToken ct)
        => Ok(await _mediator.Send(new AutoFillScheduleCommand(date), ct));

    // GET /api/schedule/suggestions
    [HttpGet("suggestions")]
    public async Task<IActionResult> GetSuggestions(CancellationToken ct)
        => Ok(await _mediator.Send(new GetScheduleSuggestionsQuery(), ct));
}
