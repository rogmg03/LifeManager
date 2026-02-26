using LifeManager.Application.Features.Labels.Commands;
using LifeManager.Application.Features.Labels.DTOs;
using LifeManager.Application.Features.Labels.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class LabelsController : ControllerBase
{
    private readonly IMediator _mediator;
    public LabelsController(IMediator mediator) => _mediator = mediator;

    // --- Label CRUD ---

    [HttpGet("api/labels")]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetLabelsQuery(), ct));

    [HttpPost("api/labels")]
    public async Task<IActionResult> Create([FromBody] CreateLabelRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateLabelCommand(request.Name, request.Color), ct);
        return StatusCode(201, result);
    }

    [HttpPut("api/labels/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLabelRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateLabelCommand(id, request.Name, request.Color), ct));

    [HttpDelete("api/labels/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteLabelCommand(id), ct);
        return NoContent();
    }

    // --- Project label associations ---

    [HttpPost("api/projects/{projectId:guid}/labels/{labelId:guid}")]
    public async Task<IActionResult> AssignToProject(Guid projectId, Guid labelId, CancellationToken ct)
    {
        await _mediator.Send(new AssignLabelToProjectCommand(projectId, labelId), ct);
        return NoContent();
    }

    [HttpDelete("api/projects/{projectId:guid}/labels/{labelId:guid}")]
    public async Task<IActionResult> RemoveFromProject(Guid projectId, Guid labelId, CancellationToken ct)
    {
        await _mediator.Send(new RemoveLabelFromProjectCommand(projectId, labelId), ct);
        return NoContent();
    }

    // --- Task label associations ---

    [HttpPost("api/tasks/{taskId:guid}/labels/{labelId:guid}")]
    public async Task<IActionResult> AssignToTask(Guid taskId, Guid labelId, CancellationToken ct)
    {
        await _mediator.Send(new AssignLabelToTaskCommand(taskId, labelId), ct);
        return NoContent();
    }

    [HttpDelete("api/tasks/{taskId:guid}/labels/{labelId:guid}")]
    public async Task<IActionResult> RemoveFromTask(Guid taskId, Guid labelId, CancellationToken ct)
    {
        await _mediator.Send(new RemoveLabelFromTaskCommand(taskId, labelId), ct);
        return NoContent();
    }
}
