using LifeManager.Application.Features.FreeTime.Commands;
using LifeManager.Application.Features.FreeTime.DTOs;
using LifeManager.Application.Features.FreeTime.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Route("api/free-time")]
[Authorize]
public class FreeTimeController : ControllerBase
{
    private readonly IMediator _mediator;
    public FreeTimeController(IMediator mediator) => _mediator = mediator;

    /// <summary>GET /api/free-time/balance — current running balance in minutes.</summary>
    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance(CancellationToken ct)
        => Ok(await _mediator.Send(new GetFreeTimeBalanceQuery(), ct));

    /// <summary>GET /api/free-time/transactions?from=&to=&page=&pageSize= — paginated transaction history.</summary>
    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetFreeTimeTransactionsQuery(from, to, page, pageSize), ct));

    /// <summary>GET /api/free-time/daily-summary?date= — worked/earned/spent summary for a calendar day.</summary>
    [HttpGet("daily-summary")]
    public async Task<IActionResult> GetDailySummary(
        [FromQuery] DateTime date,
        CancellationToken ct)
        => Ok(await _mediator.Send(new GetFreeTimeDailySummaryQuery(date), ct));

    /// <summary>POST /api/free-time/cash-out — spend free minutes (creates a Spent transaction).</summary>
    [HttpPost("cash-out")]
    public async Task<IActionResult> CashOut([FromBody] CashOutRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CashOutFreeTimeCommand(request.Minutes, request.Notes), ct);
        return Ok(result);
    }
}
