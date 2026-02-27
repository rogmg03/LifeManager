using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.ExerciseGoals.Commands;

public record DeleteExerciseGoalCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteExerciseGoalCommandHandler : IRequestHandler<DeleteExerciseGoalCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteExerciseGoalCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteExerciseGoalCommand request, CancellationToken ct)
    {
        var goal = await _uow.ExerciseGoals.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("ExerciseGoal", request.Id);

        _uow.ExerciseGoals.Delete(goal);
        await _uow.SaveChangesAsync(ct);
    }
}
