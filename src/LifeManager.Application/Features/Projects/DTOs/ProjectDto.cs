using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Projects.DTOs;

public record ProjectDto(
    Guid Id,
    Guid UserId,
    Guid? ClientId,
    string Name,
    string? Description,
    string Type,
    string Status,
    string? Color,
    DateOnly? StartDate,
    DateOnly? EndDate,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static ProjectDto FromEntity(Project p) => new(
        p.Id, p.UserId, p.ClientId,
        p.Name, p.Description,
        p.Type.ToString(), p.Status.ToString(),
        p.Color, p.StartDate, p.EndDate,
        p.CreatedAt, p.UpdatedAt);
}
