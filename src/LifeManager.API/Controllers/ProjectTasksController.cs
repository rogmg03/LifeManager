using LifeManager.Application.Features.Tasks.Commands;
using LifeManager.Application.Features.Tasks.DTOs;
using LifeManager.Application.Features.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/tasks")]
[Authorize]
public class ProjectTasksController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProjectTasksController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid projectId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTasksQuery(projectId), ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid projectId, Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetTaskByIdQuery(id), ct));

    [HttpPost]
    public async Task<IActionResult> Create(Guid projectId, [FromBody] CreateTaskRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateTaskCommand(
            projectId, request.Title, request.Description, request.Priority,
            request.PhaseId, request.DueDate, request.EstimatedMinutes, request.SortOrder), ct);
        return CreatedAtAction(nameof(GetById), new { projectId, id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid projectId, Guid id, [FromBody] UpdateTaskRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateTaskCommand(
            id, request.Title, request.Description, request.Status, request.Priority,
            request.PhaseId, request.DueDate, request.EstimatedMinutes, request.SortOrder), ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid projectId, Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteTaskCommand(id), ct);
        return NoContent();
    }
}
