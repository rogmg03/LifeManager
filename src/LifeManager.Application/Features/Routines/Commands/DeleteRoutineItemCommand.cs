using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Routines.Commands;

public record DeleteRoutineItemCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteRoutineItemCommandHandler : IRequestHandler<DeleteRoutineItemCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public DeleteRoutineItemCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task Handle(DeleteRoutineItemCommand request, CancellationToken ct)
    {
        var item = await _uow.RoutineItems.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("RoutineItem", request.Id);

        var routine = await _uow.Routines.GetByIdAsync(item.RoutineId, ct)
            ?? throw new NotFoundException("Routine", item.RoutineId);

        if (routine.UserId != _currentUser.UserId)
            throw new NotFoundException("RoutineItem", request.Id);

        _uow.RoutineItems.Delete(item);
        await _uow.SaveChangesAsync(ct);
    }
}
