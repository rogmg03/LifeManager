using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Features.Tasks.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Tasks.Commands;

public record CreateTaskCommand(
    Guid ProjectId,
    string Title,
    string? Description,
    TaskPriority Priority,
    Guid? PhaseId,
    DateTime? DueDate,
    int? EstimatedMinutes,
    int SortOrder) : IRequest<TaskDto>, IBaseCommand;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly IUnitOfWork _uow;
    public CreateTaskCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken ct)
    {
        var task = new ProjectTask
        {
            ProjectId = request.ProjectId,
            PhaseId = request.PhaseId,
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            DueDate = request.DueDate,
            EstimatedMinutes = request.EstimatedMinutes,
            SortOrder = request.SortOrder
        };
        await _uow.Tasks.AddAsync(task, ct);
        await _uow.SaveChangesAsync(ct);
        return TaskDto.FromEntity(task);
    }
}
