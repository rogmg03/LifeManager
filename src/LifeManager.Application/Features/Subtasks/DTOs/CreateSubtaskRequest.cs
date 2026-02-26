namespace LifeManager.Application.Features.Subtasks.DTOs;
public record CreateSubtaskRequest(string Title, int SortOrder = 0);
