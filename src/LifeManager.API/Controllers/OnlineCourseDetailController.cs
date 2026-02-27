using LifeManager.Application.Features.OnlineCourseDetails.Commands;
using LifeManager.Application.Features.OnlineCourseDetails.DTOs;
using LifeManager.Application.Features.OnlineCourseDetails.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/online-detail")]
[Authorize]
public class OnlineCourseDetailController : ControllerBase
{
    private readonly IMediator _mediator;
    public OnlineCourseDetailController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(Guid projectId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetOnlineCourseDetailQuery(projectId), ct));

    [HttpPost]
    public async Task<IActionResult> Create(
        Guid projectId,
        [FromBody] CreateOnlineCourseDetailRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateOnlineCourseDetailCommand(
            projectId,
            request.Platform,
            request.CourseUrl,
            request.InstructorName,
            request.TotalLessons,
            request.CompletedLessons,
            request.CertificateUrl,
            request.StartedAt,
            request.CompletedAt), ct);
        return CreatedAtAction(nameof(Get), new { projectId }, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        Guid projectId,
        [FromBody] UpdateOnlineCourseDetailRequest request,
        CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateOnlineCourseDetailCommand(
            projectId,
            request.Platform,
            request.CourseUrl,
            request.InstructorName,
            request.TotalLessons,
            request.CompletedLessons,
            request.CertificateUrl,
            request.StartedAt,
            request.CompletedAt), ct));

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid projectId, CancellationToken ct)
    {
        await _mediator.Send(new DeleteOnlineCourseDetailCommand(projectId), ct);
        return NoContent();
    }
}
