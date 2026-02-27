using LifeManager.Application.Features.ProgressEntries.Commands;
using LifeManager.Application.Features.ProgressEntries.DTOs;
using LifeManager.Application.Features.ProgressEntries.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class ProgressEntriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProgressEntriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("api/exercise-goals/{goalId:guid}/progress")]
    public async Task<IActionResult> GetByGoal(Guid goalId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProgressEntriesByGoalQuery(goalId), ct));

    [HttpPost("api/exercise-goals/{goalId:guid}/progress")]
    public async Task<IActionResult> Create(
        Guid goalId,
        [FromBody] CreateProgressEntryRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateProgressEntryCommand(
            goalId, request.RecordedAt, request.Value, request.Notes), ct);
        return CreatedAtAction(nameof(GetByGoal), new { goalId }, result);
    }

    [HttpDelete("api/progress-entries/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteProgressEntryCommand(id), ct);
        return NoContent();
    }
}
