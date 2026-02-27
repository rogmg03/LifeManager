using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Routines.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Routines.Commands;

public record ReorderRoutineItemsCommand(
    Guid RoutineId,
    IReadOnlyList<ReorderItemEntry> Items) : IRequest, IBaseCommand;

public class ReorderRoutineItemsCommandHandler : IRequestHandler<ReorderRoutineItemsCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public ReorderRoutineItemsCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task Handle(ReorderRoutineItemsCommand request, CancellationToken ct)
    {
        var routine = await _uow.Routines.GetByIdAsync(request.RoutineId, ct)
            ?? throw new NotFoundException("Routine", request.RoutineId);

        if (routine.UserId != _currentUser.UserId)
            throw new NotFoundException("Routine", request.RoutineId);

        foreach (var entry in request.Items)
        {
            var item = await _uow.RoutineItems.GetByIdAsync(entry.Id, ct);
            if (item is null || item.RoutineId != request.RoutineId) continue;

            item.SortOrder = entry.SortOrder;
            _uow.RoutineItems.Update(item);
        }

        await _uow.SaveChangesAsync(ct);
    }
}
