using LifeManager.Application.Features.Reminders.Commands;
using LifeManager.Application.Features.Reminders.DTOs;
using LifeManager.Application.Features.Reminders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Route("api/reminders")]
[Authorize]
public class RemindersController : ControllerBase
{
    private readonly IMediator _mediator;
    public RemindersController(IMediator mediator) => _mediator = mediator;

    // GET /api/reminders?pendingOnly=
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool pendingOnly = false, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetRemindersQuery(pendingOnly), ct));

    // GET /api/reminders/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetReminderByIdQuery(id), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReminderRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateReminderCommand(
            request.Title, request.ReminderType, request.RemindAt,
            request.TaskId, request.ScheduleBlockId, request.Notes), ct);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReminderRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateReminderCommand(
            id, request.Title, request.ReminderType, request.RemindAt,
            request.TaskId, request.ScheduleBlockId, request.Notes), ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteReminderCommand(id), ct);
        return NoContent();
    }

    // PUT /api/reminders/{id}/snooze
    [HttpPut("{id:guid}/snooze")]
    public async Task<IActionResult> Snooze(Guid id, [FromBody] SnoozeReminderRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new SnoozeReminderCommand(id, request.SnoozedUntil), ct));

    // PUT /api/reminders/{id}/dismiss
    [HttpPut("{id:guid}/dismiss")]
    public async Task<IActionResult> Dismiss(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DismissReminderCommand(id), ct));
}
