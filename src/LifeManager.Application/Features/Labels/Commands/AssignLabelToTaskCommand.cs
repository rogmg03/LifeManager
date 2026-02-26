using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Labels.Commands;

public record AssignLabelToTaskCommand(Guid TaskId, Guid LabelId) : IRequest, IBaseCommand;

public class AssignLabelToTaskCommandHandler : IRequestHandler<AssignLabelToTaskCommand>
{
    private readonly IUnitOfWork _uow;
    public AssignLabelToTaskCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(AssignLabelToTaskCommand request, CancellationToken ct)
    {
        var task = await _uow.Tasks.GetByIdAsync(request.TaskId, ct)
            ?? throw new NotFoundException("Task", request.TaskId);

        var label = await _uow.Labels.GetByIdAsync(request.LabelId, ct)
            ?? throw new NotFoundException("Label", request.LabelId);

        await _uow.Labels.AddTaskLabelAsync(task.Id, label.Id, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
