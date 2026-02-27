using LifeManager.Application.Features.ExerciseGoals.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.ExerciseGoals.Queries;

public record GetExerciseGoalsByProjectQuery(Guid ProjectId) : IRequest<List<ExerciseGoalDto>>;

public class GetExerciseGoalsByProjectQueryHandler : IRequestHandler<GetExerciseGoalsByProjectQuery, List<ExerciseGoalDto>>
{
    private readonly IUnitOfWork _uow;
    public GetExerciseGoalsByProjectQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<List<ExerciseGoalDto>> Handle(GetExerciseGoalsByProjectQuery request, CancellationToken ct)
    {
        var goals = await _uow.ExerciseGoals.GetByProjectIdAsync(request.ProjectId, ct);
        return goals.Select(ExerciseGoalDto.FromEntity).ToList();
    }
}
