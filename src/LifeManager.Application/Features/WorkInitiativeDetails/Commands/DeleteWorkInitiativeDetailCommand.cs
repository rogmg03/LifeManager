using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.WorkInitiativeDetails.Commands;

public record DeleteWorkInitiativeDetailCommand(Guid ProjectId) : IRequest, IBaseCommand;

public class DeleteWorkInitiativeDetailCommandHandler : IRequestHandler<DeleteWorkInitiativeDetailCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteWorkInitiativeDetailCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteWorkInitiativeDetailCommand request, CancellationToken ct)
    {
        var detail = await _uow.WorkInitiativeDetails.GetByProjectIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("WorkInitiativeDetail", request.ProjectId);

        _uow.WorkInitiativeDetails.Delete(detail);
        await _uow.SaveChangesAsync(ct);
    }
}
