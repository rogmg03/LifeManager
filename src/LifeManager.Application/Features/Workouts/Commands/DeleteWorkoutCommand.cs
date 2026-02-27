using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Workouts.Commands;

public record DeleteWorkoutCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteWorkoutCommandHandler : IRequestHandler<DeleteWorkoutCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public DeleteWorkoutCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task Handle(DeleteWorkoutCommand request, CancellationToken ct)
    {
        var session = await _uow.WorkoutSessions.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("WorkoutSession", request.Id);

        if (session.UserId != _currentUser.UserId)
            throw new NotFoundException("WorkoutSession", request.Id);

        _uow.WorkoutSessions.Delete(session);
        await _uow.SaveChangesAsync(ct);
    }
}
