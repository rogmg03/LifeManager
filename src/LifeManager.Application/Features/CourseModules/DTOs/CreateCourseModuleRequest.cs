namespace LifeManager.Application.Features.CourseModules.DTOs;

public record CreateCourseModuleRequest(
    string Name,
    string? Description,
    int SortOrder,
    string? Notes);
