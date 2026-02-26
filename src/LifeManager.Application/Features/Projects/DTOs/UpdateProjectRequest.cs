using LifeManager.Domain.Enums;
namespace LifeManager.Application.Features.Projects.DTOs;

public record UpdateProjectRequest(
    string Name,
    string? Description,
    ProjectType Type,
    ProjectStatus Status,
    Guid? ClientId,
    string? Color,
    DateOnly? StartDate,
    DateOnly? EndDate);
