using LifeManager.Application.Common.Behaviors;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Labels.Commands;

public record RemoveLabelFromTaskCommand(Guid TaskId, Guid LabelId) : IRequest, IBaseCommand;

public class RemoveLabelFromTaskCommandHandler : IRequestHandler<RemoveLabelFromTaskCommand>
{
    private readonly IUnitOfWork _uow;
    public RemoveLabelFromTaskCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(RemoveLabelFromTaskCommand request, CancellationToken ct)
    {
        await _uow.Labels.RemoveTaskLabelAsync(request.TaskId, request.LabelId, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
