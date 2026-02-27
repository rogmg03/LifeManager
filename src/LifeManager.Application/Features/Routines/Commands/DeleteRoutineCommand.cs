using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Routines.Commands;

public record DeleteRoutineCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteRoutineCommandHandler : IRequestHandler<DeleteRoutineCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteRoutineCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteRoutineCommand request, CancellationToken ct)
    {
        var routine = await _uow.Routines.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Routine", request.Id);

        _uow.Routines.Delete(routine);
        await _uow.SaveChangesAsync(ct);
    }
}
