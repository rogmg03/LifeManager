using LifeManager.Application.Features.WorkInitiativeDetails.Commands;
using LifeManager.Application.Features.WorkInitiativeDetails.DTOs;
using LifeManager.Application.Features.WorkInitiativeDetails.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/work-detail")]
[Authorize]
public class WorkInitiativeDetailController : ControllerBase
{
    private readonly IMediator _mediator;
    public WorkInitiativeDetailController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(Guid projectId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetWorkInitiativeDetailQuery(projectId), ct));

    [HttpPost]
    public async Task<IActionResult> Create(
        Guid projectId,
        [FromBody] CreateWorkInitiativeDetailRequest request,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateWorkInitiativeDetailCommand(
            projectId,
            request.ClientName,
            request.ContractValue,
            request.BillingType,
            request.HourlyRate,
            request.EstimatedHours,
            request.LoggedHours,
            request.IsInternal), ct);
        return CreatedAtAction(nameof(Get), new { projectId }, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        Guid projectId,
        [FromBody] UpdateWorkInitiativeDetailRequest request,
        CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateWorkInitiativeDetailCommand(
            projectId,
            request.ClientName,
            request.ContractValue,
            request.BillingType,
            request.HourlyRate,
            request.EstimatedHours,
            request.LoggedHours,
            request.IsInternal), ct));

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid projectId, CancellationToken ct)
    {
        await _mediator.Send(new DeleteWorkInitiativeDetailCommand(projectId), ct);
        return NoContent();
    }
}
