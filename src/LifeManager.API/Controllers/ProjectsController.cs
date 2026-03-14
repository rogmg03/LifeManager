using LifeManager.Application.Features.Projects.Commands;
using LifeManager.Application.Features.Projects.DTOs;
using LifeManager.Application.Features.Projects.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProjectsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectsQuery(), ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectByIdQuery(id), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateProjectCommand(
            request.Name, request.Description, request.Type,
            request.ClientId, request.Color, request.StartDate, request.EndDate), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateProjectCommand(
            id, request.Name, request.Description, request.Type, request.Status,
            request.ClientId, request.Color, request.StartDate, request.EndDate), ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteProjectCommand(id), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}/permanent")]
    public async Task<IActionResult> PermanentDelete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new PermanentDeleteProjectCommand(id), ct);
        return NoContent();
    }
}
