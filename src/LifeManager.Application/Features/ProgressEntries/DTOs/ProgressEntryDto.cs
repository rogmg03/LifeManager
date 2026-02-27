using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.ProgressEntries.DTOs;

public record ProgressEntryDto(
    Guid Id,
    Guid GoalId,
    DateTimeOffset RecordedAt,
    decimal Value,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static ProgressEntryDto FromEntity(ProgressEntry e) => new(
        e.Id, e.GoalId, e.RecordedAt, e.Value, e.Notes, e.CreatedAt, e.UpdatedAt);
}
