using LifeManager.Application.Features.AssignmentDetails.Commands;
using LifeManager.Application.Features.AssignmentDetails.DTOs;
using LifeManager.Application.Features.AssignmentDetails.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class AssignmentDetailController : ControllerBase
{
    private readonly IMediator _mediator;
    public AssignmentDetailController(IMediator mediator) => _mediator = mediator;

    [HttpGet("api/tasks/{taskId:guid}/assignment-detail")]
    public async Task<IActionResult> Get(Guid taskId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetAssignmentDetailQuery(taskId), ct));

    [HttpPut("api/tasks/{taskId:guid}/assignment-detail")]
    public async Task<IActionResult> Upsert(
        Guid taskId,
        [FromBody] UpsertAssignmentDetailRequest request,
        CancellationToken ct)
        => Ok(await _mediator.Send(new UpsertAssignmentDetailCommand(
            taskId,
            request.AssignmentType,
            request.Grade,
            request.GradeLetter,
            request.Weight,
            request.SubmissionLink), ct));

    [HttpDelete("api/tasks/{taskId:guid}/assignment-detail")]
    public async Task<IActionResult> Delete(Guid taskId, CancellationToken ct)
    {
        await _mediator.Send(new DeleteAssignmentDetailCommand(taskId), ct);
        return NoContent();
    }
}
