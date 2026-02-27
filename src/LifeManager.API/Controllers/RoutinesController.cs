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

    [HttpGet("api/routines")]
    public async Task<IActionResult> List([FromQuery] bool includeArchived = false, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetRoutinesQuery(includeArchived), ct));

    [HttpPost("api/routines")]
    public async Task<IActionResult> Create([FromBody] CreateRoutineRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateRoutineCommand(
            request.Name, request.Description, request.EstimatedDurationMinutes,
            request.Category, request.SortOrder), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("api/routines/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRoutineByIdQuery(id), ct));

    [HttpPut("api/routines/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoutineRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateRoutineCommand(
            id, request.Name, request.Description, request.EstimatedDurationMinutes,
            request.Category, request.SortOrder), ct));

    [HttpDelete("api/routines/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteRoutineCommand(id), ct);
        return NoContent();
    }

    [HttpPut("api/routines/{id:guid}/archive")]
    public async Task<IActionResult> Archive(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new ArchiveRoutineCommand(id), ct));

    // Routine Items
    [HttpPost("api/routines/{routineId:guid}/items")]
    public async Task<IActionResult> AddItem(
        Guid routineId,
        [FromBody] CreateRoutineItemRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateRoutineItemCommand(
            routineId, request.ExerciseName, request.Description,
            request.TargetSets, request.TargetReps, request.TargetWeight,
            request.RestSeconds, request.SortOrder), ct);
        return CreatedAtAction(nameof(GetById), new { id = routineId }, result);
    }

    [HttpPut("api/routine-items/{id:guid}")]
    public async Task<IActionResult> UpdateItem(Guid id, [FromBody] UpdateRoutineItemRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateRoutineItemCommand(
            id, request.ExerciseName, request.Description,
            request.TargetSets, request.TargetReps, request.TargetWeight,
            request.RestSeconds, request.SortOrder), ct));

    [HttpDelete("api/routine-items/{id:guid}")]
    public async Task<IActionResult> DeleteItem(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteRoutineItemCommand(id), ct);
        return NoContent();
    }

    [HttpPut("api/routines/{routineId:guid}/items/reorder")]
    public async Task<IActionResult> ReorderItems(
        Guid routineId,
        [FromBody] ReorderItemsRequest request,
        CancellationToken ct)
    {
        await _mediator.Send(new ReorderRoutineItemsCommand(routineId, request.Items), ct);
        return NoContent();
    }
}
