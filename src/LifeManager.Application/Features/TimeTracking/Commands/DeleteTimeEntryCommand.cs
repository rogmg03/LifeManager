using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.TimeTracking.Commands;

public record DeleteTimeEntryCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteTimeEntryCommandHandler : IRequestHandler<DeleteTimeEntryCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public DeleteTimeEntryCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task Handle(DeleteTimeEntryCommand request, CancellationToken ct)
    {
        var entry = await _uow.TimeEntries.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("TimeEntry", request.Id);

        if (entry.UserId != _currentUser.UserId)
            throw new UnauthorizedAccessException("You do not have permission to delete this time entry.");

        // TODO (Cycle 10): If entry.DurationMinutes has value, reverse the earned FreeTimeTransaction
        _uow.TimeEntries.Delete(entry);
        await _uow.SaveChangesAsync(ct);
    }
}
