using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Phases.Commands;

public record DeletePhaseCommand(Guid Id) : IRequest, IBaseCommand;

public class DeletePhaseCommandHandler : IRequestHandler<DeletePhaseCommand>
{
    private readonly IUnitOfWork _uow;
    public DeletePhaseCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeletePhaseCommand request, CancellationToken ct)
    {
        var phase = await _uow.Phases.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Phase", request.Id);

        _uow.Phases.Delete(phase);
        await _uow.SaveChangesAsync(ct);
    }
}
