namespace LifeManager.Application.Features.ProgressEntries.DTOs;

public record CreateProgressEntryRequest(
    DateTimeOffset RecordedAt,
    decimal Value,
    string? Notes);
