using LifeManager.Application.Features.Routines.Commands;
using LifeManager.Application.Features.Routines.DTOs;
using LifeManager.Application.Features.Routines.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class RoutinesController : ControllerBase
{
    private readonly IMediator _mediator;
    public RoutinesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("api/projects/{projectId:guid}/routines")]
    public async Task<IActionResult> GetByProject(Guid projectId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRoutinesByProjectQuery(projectId), ct));

    [HttpPost("api/projects/{projectId:guid}/routines")]
    public async Task<IActionResult> Create(
        Guid projectId,
        [FromBody] CreateRoutineRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateRoutineCommand(
            projectId, request.Name, request.Description,
            request.DayOfWeek, request.SortOrder), ct);
        return CreatedAtAction(nameof(GetByProject), new { projectId }, result);
    }

    [HttpPut("api/routines/{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateRoutineRequest request,
        CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateRoutineCommand(
            id, request.Name, request.Description,
            request.DayOfWeek, request.SortOrder), ct));

    [HttpDelete("api/routines/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteRoutineCommand(id), ct);
        return NoContent();
    }
}
