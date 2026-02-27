using LifeManager.Application.Features.Workouts.Commands;
using LifeManager.Application.Features.Workouts.DTOs;
using LifeManager.Application.Features.Workouts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class WorkoutsController : ControllerBase
{
    private readonly IMediator _mediator;
    public WorkoutsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("api/workouts/start")]
    public async Task<IActionResult> Start([FromBody] StartWorkoutRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new StartWorkoutCommand(request.RoutineId), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("api/workouts/stats")]
    public async Task<IActionResult> GetStats(CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkoutStatsQuery(), ct));

    [HttpGet("api/workouts/exercise-progress")]
    public async Task<IActionResult> GetExerciseProgress([FromQuery] string exerciseName, CancellationToken ct)
        => Ok(await _mediator.Send(new GetExerciseProgressQuery(exerciseName), ct));

    [HttpGet("api/workouts")]
    public async Task<IActionResult> List(
        [FromQuery] Guid? routineId,
        [FromQuery] DateTimeOffset? from,
        [FromQuery] DateTimeOffset? to,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetWorkoutsQuery(routineId, from, to, page, pageSize), ct));

    [HttpGet("api/workouts/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkoutByIdQuery(id), ct));

    [HttpPut("api/workouts/{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteWorkoutRequest? request, CancellationToken ct)
        => Ok(await _mediator.Send(new CompleteWorkoutCommand(id, request?.Notes), ct));

    [HttpDelete("api/workouts/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteWorkoutCommand(id), ct);
        return NoContent();
    }

    [HttpPut("api/workout-sets/{id:guid}")]
    public async Task<IActionResult> LogSet(Guid id, [FromBody] LogSetRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new LogSetCommand(id, request.ActualReps, request.ActualWeight, request.IsCompleted), ct));

    [HttpPut("api/workout-sets/{id:guid}/skip")]
    public async Task<IActionResult> SkipSet(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new SkipSetCommand(id), ct));
}

public record CompleteWorkoutRequest(string? Notes);
