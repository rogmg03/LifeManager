using LifeManager.Application.Features.Phases.Commands;
using LifeManager.Application.Features.Phases.DTOs;
using LifeManager.Application.Features.Phases.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/phases")]
[Authorize]
public class PhasesController : ControllerBase
{
    private readonly IMediator _mediator;
    public PhasesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid projectId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPhasesQuery(projectId), ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid projectId, Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetPhaseByIdQuery(id), ct));

    [HttpPost]
    public async Task<IActionResult> Create(Guid projectId, [FromBody] CreatePhaseRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreatePhaseCommand(
            projectId, request.Name, request.Description,
            request.SortOrder, request.Priority, request.StartDate, request.EndDate), ct);
        return CreatedAtAction(nameof(GetById), new { projectId, id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid projectId, Guid id, [FromBody] UpdatePhaseRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdatePhaseCommand(
            id, request.Name, request.Description, request.SortOrder,
            request.Priority, request.Status, request.StartDate, request.EndDate), ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid projectId, Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeletePhaseCommand(id), ct);
        return NoContent();
    }
}
