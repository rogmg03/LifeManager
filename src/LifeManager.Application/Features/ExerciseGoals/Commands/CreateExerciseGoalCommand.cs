using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Features.ExerciseGoals.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.ExerciseGoals.Commands;

public record CreateExerciseGoalCommand(
    Guid ProjectId,
    string MetricName,
    decimal TargetValue,
    string Unit,
    DateOnly? Deadline) : IRequest<ExerciseGoalDto>, IBaseCommand;

public class CreateExerciseGoalCommandHandler : IRequestHandler<CreateExerciseGoalCommand, ExerciseGoalDto>
{
    private readonly IUnitOfWork _uow;
    public CreateExerciseGoalCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ExerciseGoalDto> Handle(CreateExerciseGoalCommand request, CancellationToken ct)
    {
        var goal = new ExerciseGoal
        {
            ProjectId = request.ProjectId,
            MetricName = request.MetricName,
            TargetValue = request.TargetValue,
            Unit = request.Unit,
            Deadline = request.Deadline
        };

        await _uow.ExerciseGoals.AddAsync(goal, ct);
        await _uow.SaveChangesAsync(ct);

        return ExerciseGoalDto.FromEntity(goal);
    }
}
