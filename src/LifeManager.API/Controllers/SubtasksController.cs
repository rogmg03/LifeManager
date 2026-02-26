using LifeManager.Application.Features.Subtasks.Commands;
using LifeManager.Application.Features.Subtasks.DTOs;
using LifeManager.Application.Features.Subtasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Route("api/tasks/{taskId:guid}/subtasks")]
[Authorize]
public class SubtasksController : ControllerBase
{
    private readonly IMediator _mediator;
    public SubtasksController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid taskId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetSubtasksQuery(taskId), ct));

    [HttpPost]
    public async Task<IActionResult> Create(Guid taskId, [FromBody] CreateSubtaskRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateSubtaskCommand(taskId, request.Title, request.SortOrder), ct);
        return Created($"api/tasks/{taskId}/subtasks/{result.Id}", result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid taskId, Guid id, [FromBody] UpdateSubtaskRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateSubtaskCommand(id, request.Title, request.IsCompleted, request.SortOrder), ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid taskId, Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteSubtaskCommand(id), ct);
        return NoContent();
    }
}
