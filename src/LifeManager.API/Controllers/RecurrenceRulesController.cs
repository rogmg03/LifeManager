using LifeManager.Application.Features.RecurrenceRules.Commands;
using LifeManager.Application.Features.RecurrenceRules.DTOs;
using LifeManager.Application.Features.RecurrenceRules.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Route("api/tasks/{taskId:guid}/recurrence")]
[Authorize]
public class RecurrenceRulesController : ControllerBase
{
    private readonly IMediator _mediator;
    public RecurrenceRulesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(Guid taskId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetRecurrenceRuleQuery(taskId), ct));

    [HttpPost]
    public async Task<IActionResult> Create(Guid taskId, [FromBody] CreateRecurrenceRuleRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateRecurrenceRuleCommand(
            taskId, request.Pattern, request.NextDueDate, request.IntervalDays, request.DaysOfWeek), ct);
        return Created($"api/tasks/{taskId}/recurrence", result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(Guid taskId, [FromBody] UpdateRecurrenceRuleRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateRecurrenceRuleCommand(
            taskId, request.Pattern, request.NextDueDate, request.IntervalDays, request.DaysOfWeek, request.IsActive), ct));

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid taskId, CancellationToken ct)
    {
        await _mediator.Send(new DeleteRecurrenceRuleCommand(taskId), ct);
        return NoContent();
    }
}
