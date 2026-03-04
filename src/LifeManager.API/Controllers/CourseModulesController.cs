using LifeManager.Application.Features.CourseModules.Commands;
using LifeManager.Application.Features.CourseModules.DTOs;
using LifeManager.Application.Features.CourseModules.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class CourseModulesController : ControllerBase
{
    private readonly IMediator _mediator;
    public CourseModulesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("api/projects/{projectId:guid}/modules")]
    public async Task<IActionResult> GetByProject(Guid projectId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCourseModulesQuery(projectId), ct));

    [HttpPost("api/projects/{projectId:guid}/modules")]
    public async Task<IActionResult> Create(
        Guid projectId,
        [FromBody] CreateCourseModuleRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateCourseModuleCommand(
            projectId,
            request.Name,
            request.Description,
            request.SortOrder,
            request.Notes), ct);
        return CreatedAtAction(nameof(GetByProject), new { projectId }, result);
    }

    [HttpPut("api/modules/{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCourseModuleRequest request,
        CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateCourseModuleCommand(
            id,
            request.Name,
            request.Description,
            request.IsCompleted,
            request.Notes), ct));

    [HttpPut("api/modules/reorder")]
    public async Task<IActionResult> Reorder(
        [FromBody] ReorderCourseModulesRequest request,
        CancellationToken ct)
    {
        var items = request.Items.Select(i => (i.Id, i.SortOrder)).ToList();
        await _mediator.Send(new ReorderCourseModulesCommand(items), ct);
        return NoContent();
    }

    [HttpDelete("api/modules/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteCourseModuleCommand(id), ct);
        return NoContent();
    }
}
