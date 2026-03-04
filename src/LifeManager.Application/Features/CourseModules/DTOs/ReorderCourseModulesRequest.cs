namespace LifeManager.Application.Features.CourseModules.DTOs;

public record ReorderCourseModulesRequest(List<CourseModuleOrderItem> Items);

public record CourseModuleOrderItem(Guid Id, int SortOrder);
