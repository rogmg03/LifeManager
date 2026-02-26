using LifeManager.Domain.Enums;
namespace LifeManager.Application.Features.Projects.DTOs;

public record CreateProjectRequest(
    string Name,
    string? Description,
    ProjectType Type,
    Guid? ClientId,
    string? Color,
    DateOnly? StartDate,
    DateOnly? EndDate);
