using LifeManager.Application.Features.CollegeCourseDetails.Commands;
using LifeManager.Application.Features.CollegeCourseDetails.DTOs;
using LifeManager.Application.Features.CollegeCourseDetails.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/college-detail")]
[Authorize]
public class CollegeCourseDetailController : ControllerBase
{
    private readonly IMediator _mediator;
    public CollegeCourseDetailController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(Guid projectId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetCollegeCourseDetailQuery(projectId), ct));

    [HttpPost]
    public async Task<IActionResult> Create(
        Guid projectId,
        [FromBody] CreateCollegeCourseDetailRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateCollegeCourseDetailCommand(
            projectId,
            request.InstitutionName,
            request.CourseName,
            request.CourseCode,
            request.Semester,
            request.Year,
            request.Credits,
            request.Professor,
            request.Room,
            request.Schedule,
            request.CurrentGrade,
            request.TargetGrade), ct);
        return CreatedAtAction(nameof(Get), new { projectId }, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        Guid projectId,
        [FromBody] UpdateCollegeCourseDetailRequest request,
        CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateCollegeCourseDetailCommand(
            projectId,
            request.InstitutionName,
            request.CourseName,
            request.CourseCode,
            request.Semester,
            request.Year,
            request.Credits,
            request.Professor,
            request.Room,
            request.Schedule,
            request.CurrentGrade,
            request.TargetGrade), ct));

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid projectId, CancellationToken ct)
    {
        await _mediator.Send(new DeleteCollegeCourseDetailCommand(projectId), ct);
        return NoContent();
    }
}
