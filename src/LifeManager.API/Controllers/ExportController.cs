using System.Text;
using System.Text.Json;
using LifeManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeManager.API.Controllers;

[ApiController]
[Authorize]
public class ExportController : ControllerBase
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private static readonly HashSet<string> ValidEntities =
    [
        "projects", "tasks", "clients", "time-entries",
        "schedule-blocks", "labels", "documents"
    ];

    private readonly IExportService _exportService;
    private readonly ICurrentUserService _currentUserService;

    public ExportController(IExportService exportService, ICurrentUserService currentUserService)
    {
        _exportService = exportService;
        _currentUserService = currentUserService;
    }

    // GET /api/export/json
    [HttpGet("api/export/json")]
    public async Task<IActionResult> ExportJson(CancellationToken ct = default)
    {
        var data = await _exportService.GetAllDataAsync(_currentUserService.UserId, ct);
        var json = JsonSerializer.Serialize(data, JsonOptions);
        var bytes = Encoding.UTF8.GetBytes(json);
        return File(bytes, "application/json", "lifemanager-export.json");
    }

    // GET /api/export/csv?entity=
    [HttpGet("api/export/csv")]
    public async Task<IActionResult> ExportCsv(
        [FromQuery] string? entity,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(entity))
            return BadRequest(new { error = "Query parameter 'entity' is required.", validValues = ValidEntities });

        var key = entity.ToLowerInvariant().Trim();
        if (!ValidEntities.Contains(key))
            return BadRequest(new { error = $"Unknown entity '{entity}'.", validValues = ValidEntities });

        var csv = await _exportService.GetCsvAsync(_currentUserService.UserId, key, ct);
        var bytes = Encoding.UTF8.GetBytes(csv);
        return File(bytes, "text/csv", $"lifemanager-{key}.csv");
    }
}
