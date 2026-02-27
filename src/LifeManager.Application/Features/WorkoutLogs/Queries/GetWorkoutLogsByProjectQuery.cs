using LifeManager.Application.Features.WorkoutLogs.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.WorkoutLogs.Queries;

public record GetWorkoutLogsByProjectQuery(Guid ProjectId) : IRequest<List<WorkoutLogDto>>;

public class GetWorkoutLogsByProjectQueryHandler : IRequestHandler<GetWorkoutLogsByProjectQuery, List<WorkoutLogDto>>
{
    private readonly IUnitOfWork _uow;
    public GetWorkoutLogsByProjectQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<List<WorkoutLogDto>> Handle(GetWorkoutLogsByProjectQuery request, CancellationToken ct)
    {
        var logs = await _uow.WorkoutLogs.GetByProjectIdAsync(request.ProjectId, ct);
        return logs.Select(WorkoutLogDto.FromEntity).ToList();
    }
}
