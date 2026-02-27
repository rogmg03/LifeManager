using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.ExerciseGoals.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.ExerciseGoals.Commands;

public record UpdateExerciseGoalCommand(
    Guid Id,
    string MetricName,
    decimal TargetValue,
    string Unit,
    DateOnly? Deadline) : IRequest<ExerciseGoalDto>, IBaseCommand;

public class UpdateExerciseGoalCommandHandler : IRequestHandler<UpdateExerciseGoalCommand, ExerciseGoalDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateExerciseGoalCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ExerciseGoalDto> Handle(UpdateExerciseGoalCommand request, CancellationToken ct)
    {
        var goal = await _uow.ExerciseGoals.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("ExerciseGoal", request.Id);

        goal.MetricName = request.MetricName;
        goal.TargetValue = request.TargetValue;
        goal.Unit = request.Unit;
        goal.Deadline = request.Deadline;

        _uow.ExerciseGoals.Update(goal);
        await _uow.SaveChangesAsync(ct);

        return ExerciseGoalDto.FromEntity(goal);
    }
}
