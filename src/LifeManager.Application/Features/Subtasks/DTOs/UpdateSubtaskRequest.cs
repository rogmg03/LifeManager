namespace LifeManager.Application.Features.Subtasks.DTOs;
public record UpdateSubtaskRequest(string Title, bool IsCompleted, int SortOrder);
