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

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool pendingOnly = false, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetRemindersQuery(pendingOnly), ct));

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

    [HttpPut("{id:guid}/dismiss")]
    public async Task<IActionResult> Dismiss(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new DismissReminderCommand(id), ct));
}
