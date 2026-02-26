using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Activity.DTOs;

public record ActivityEntryDto(
    Guid Id,
    Guid UserId,
    Guid? ProjectId,
    string ActivityType,
    string Description,
    Guid? EntityId,
    string? EntityType,
    DateTime CreatedAt)
{
    public static ActivityEntryDto FromEntity(ActivityEntry a) => new(
        a.Id,
        a.UserId,
        a.ProjectId,
        a.ActivityType.ToString(),
        a.Description,
        a.EntityId,
        a.EntityType,
        a.CreatedAt);
}
