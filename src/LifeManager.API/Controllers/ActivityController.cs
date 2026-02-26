using LifeManager.Application.Features.Activity.Commands;
using LifeManager.Application.Features.Activity.DTOs;
using LifeManager.Application.Features.Activity.Queries;
using LifeManager.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class ActivityController : ControllerBase
{
    private readonly IMediator _mediator;
    public ActivityController(IMediator mediator) => _mediator = mediator;

    // GET /api/activity?projectId=&type=&from=&to=&page=&pageSize=
    [HttpGet("api/activity")]
    public async Task<IActionResult> GetFeed(
        [FromQuery] Guid? projectId,
        [FromQuery] ActivityType? type,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
        => Ok(await _mediator.Send(
            new GetActivityFeedQuery(projectId, type, from, to, page, pageSize), ct));

    // GET /api/projects/{projectId}/activity
    [HttpGet("api/projects/{projectId:guid}/activity")]
    public async Task<IActionResult> GetByProject(Guid projectId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetProjectActivityQuery(projectId), ct));

    // POST /api/projects/{projectId}/activity
    [HttpPost("api/projects/{projectId:guid}/activity")]
    public async Task<IActionResult> PostManual(
        Guid projectId,
        [FromBody] CreateActivityEntryRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(
            new CreateActivityEntryCommand(projectId, request.Description), ct);
        return StatusCode(201, result);
    }
}
