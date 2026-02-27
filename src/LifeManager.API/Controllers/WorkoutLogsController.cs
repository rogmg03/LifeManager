using LifeManager.Application.Features.WorkoutLogs.Commands;
using LifeManager.Application.Features.WorkoutLogs.DTOs;
using LifeManager.Application.Features.WorkoutLogs.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class WorkoutLogsController : ControllerBase
{
    private readonly IMediator _mediator;
    public WorkoutLogsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("api/projects/{projectId:guid}/workout-logs")]
    public async Task<IActionResult> GetByProject(Guid projectId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkoutLogsByProjectQuery(projectId), ct));

    [HttpPost("api/projects/{projectId:guid}/workout-logs")]
    public async Task<IActionResult> Create(
        Guid projectId,
        [FromBody] CreateWorkoutLogRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateWorkoutLogCommand(
            projectId, request.RoutineId, request.LoggedAt,
            request.Notes, request.DurationMinutes), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("api/workout-logs/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkoutLogByIdQuery(id), ct));

    [HttpDelete("api/workout-logs/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteWorkoutLogCommand(id), ct);
        return NoContent();
    }
}
