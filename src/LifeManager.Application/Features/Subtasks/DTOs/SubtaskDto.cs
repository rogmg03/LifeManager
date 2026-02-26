using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Subtasks.DTOs;

public record SubtaskDto(Guid Id, Guid TaskId, string Title, bool IsCompleted, int SortOrder)
{
    public static SubtaskDto FromEntity(Subtask s) => new(s.Id, s.TaskId, s.Title, s.IsCompleted, s.SortOrder);
}
