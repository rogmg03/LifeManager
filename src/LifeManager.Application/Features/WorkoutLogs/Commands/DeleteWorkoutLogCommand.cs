using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.WorkoutLogs.Commands;

public record DeleteWorkoutLogCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteWorkoutLogCommandHandler : IRequestHandler<DeleteWorkoutLogCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteWorkoutLogCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteWorkoutLogCommand request, CancellationToken ct)
    {
        var log = await _uow.WorkoutLogs.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("WorkoutLog", request.Id);

        _uow.WorkoutLogs.Delete(log);
        await _uow.SaveChangesAsync(ct);
    }
}
