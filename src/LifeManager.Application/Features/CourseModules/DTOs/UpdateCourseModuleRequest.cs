namespace LifeManager.Application.Features.CourseModules.DTOs;

public record UpdateCourseModuleRequest(
    string Name,
    string? Description,
    bool IsCompleted,
    string? Notes);
