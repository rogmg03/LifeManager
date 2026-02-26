using LifeManager.Application.Common.Behaviors;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Labels.Commands;

public record RemoveLabelFromProjectCommand(Guid ProjectId, Guid LabelId) : IRequest, IBaseCommand;

public class RemoveLabelFromProjectCommandHandler : IRequestHandler<RemoveLabelFromProjectCommand>
{
    private readonly IUnitOfWork _uow;
    public RemoveLabelFromProjectCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(RemoveLabelFromProjectCommand request, CancellationToken ct)
    {
        await _uow.Labels.RemoveProjectLabelAsync(request.ProjectId, request.LabelId, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
