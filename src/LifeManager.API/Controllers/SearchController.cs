using LifeManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class SearchController : ControllerBase
{
    private readonly ISearchReadService _searchReadService;
    private readonly ICurrentUserService _currentUserService;

    public SearchController(
        ISearchReadService searchReadService,
        ICurrentUserService currentUserService)
    {
        _searchReadService = searchReadService;
        _currentUserService = currentUserService;
    }

    // GET /api/search?q=&type=
    [HttpGet("api/search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? q,
        [FromQuery] string? type,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(new { error = "Query parameter 'q' is required." });

        var results = await _searchReadService.SearchAsync(
            _currentUserService.UserId,
            q.Trim(),
            type?.ToLowerInvariant(),
            ct);

        return Ok(results);
    }
}
