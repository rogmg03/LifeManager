using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Workouts.DTOs;
using MediatR;

namespace LifeManager.Application.Features.Workouts.Queries;

public record GetWorkoutStatsQuery : IRequest<WorkoutStatsDto>;

public class GetWorkoutStatsQueryHandler : IRequestHandler<GetWorkoutStatsQuery, WorkoutStatsDto>
{
    private readonly IWorkoutReadService _workoutReadService;
    private readonly ICurrentUserService _currentUser;

    public GetWorkoutStatsQueryHandler(IWorkoutReadService workoutReadService, ICurrentUserService currentUser)
    {
        _workoutReadService = workoutReadService;
        _currentUser = currentUser;
    }

    public async Task<WorkoutStatsDto> Handle(GetWorkoutStatsQuery request, CancellationToken ct)
        => await _workoutReadService.GetWorkoutStatsAsync(_currentUser.UserId, ct);
}
