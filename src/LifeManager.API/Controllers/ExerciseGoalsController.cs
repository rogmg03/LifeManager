using LifeManager.Application.Features.ExerciseGoals.Commands;
using LifeManager.Application.Features.ExerciseGoals.DTOs;
using LifeManager.Application.Features.ExerciseGoals.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class ExerciseGoalsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ExerciseGoalsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("api/projects/{projectId:guid}/exercise-goals")]
    public async Task<IActionResult> GetByProject(Guid projectId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetExerciseGoalsByProjectQuery(projectId), ct));

    [HttpPost("api/projects/{projectId:guid}/exercise-goals")]
    public async Task<IActionResult> Create(
        Guid projectId,
        [FromBody] CreateExerciseGoalRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateExerciseGoalCommand(
            projectId, request.MetricName, request.TargetValue,
            request.Unit, request.Deadline), ct);
        return CreatedAtAction(nameof(GetByProject), new { projectId }, result);
    }

    [HttpPut("api/exercise-goals/{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateExerciseGoalRequest request,
        CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateExerciseGoalCommand(
            id, request.MetricName, request.TargetValue,
            request.Unit, request.Deadline), ct));

    [HttpDelete("api/exercise-goals/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteExerciseGoalCommand(id), ct);
        return NoContent();
    }
}
