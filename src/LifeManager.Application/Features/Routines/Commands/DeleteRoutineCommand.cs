using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Routines.Commands;

public record DeleteRoutineCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteRoutineCommandHandler : IRequestHandler<DeleteRoutineCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public DeleteRoutineCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task Handle(DeleteRoutineCommand request, CancellationToken ct)
    {
        var routine = await _uow.Routines.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Routine", request.Id);

        if (routine.UserId != _currentUser.UserId)
            throw new NotFoundException("Routine", request.Id);

        _uow.Routines.Delete(routine);
        await _uow.SaveChangesAsync(ct);
    }
}
