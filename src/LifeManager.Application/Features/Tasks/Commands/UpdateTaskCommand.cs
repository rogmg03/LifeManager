using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Tasks.DTOs;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;
using TaskStatus = LifeManager.Domain.Enums.TaskStatus;

namespace LifeManager.Application.Features.Tasks.Commands;

public record UpdateTaskCommand(
    Guid Id,
    string Title,
    string? Description,
    TaskStatus Status,
    TaskPriority Priority,
    Guid? PhaseId,
    DateTime? DueDate,
    int? EstimatedMinutes,
    int SortOrder) : IRequest<TaskDto>, IBaseCommand;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateTaskCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken ct)
    {
        var task = await _uow.Tasks.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Task", request.Id);

        var wasCompleted = task.Status == TaskStatus.Done;
        var isNowCompleted = request.Status == TaskStatus.Done;

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.Priority = request.Priority;
        task.PhaseId = request.PhaseId;
        task.DueDate = request.DueDate;
        task.EstimatedMinutes = request.EstimatedMinutes;
        task.SortOrder = request.SortOrder;

        if (!wasCompleted && isNowCompleted)
            task.CompletedAt = DateTime.UtcNow;
        else if (wasCompleted && !isNowCompleted)
            task.CompletedAt = null;

        _uow.Tasks.Update(task);
        await _uow.SaveChangesAsync(ct);
        return TaskDto.FromEntity(task);
    }
}
