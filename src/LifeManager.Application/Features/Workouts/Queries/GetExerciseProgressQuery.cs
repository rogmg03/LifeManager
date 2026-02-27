using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Workouts.DTOs;
using MediatR;

namespace LifeManager.Application.Features.Workouts.Queries;

public record GetExerciseProgressQuery(string ExerciseName) : IRequest<ExerciseProgressDto>;

public class GetExerciseProgressQueryHandler : IRequestHandler<GetExerciseProgressQuery, ExerciseProgressDto>
{
    private readonly IWorkoutReadService _workoutReadService;
    private readonly ICurrentUserService _currentUser;

    public GetExerciseProgressQueryHandler(IWorkoutReadService workoutReadService, ICurrentUserService currentUser)
    {
        _workoutReadService = workoutReadService;
        _currentUser = currentUser;
    }

    public async Task<ExerciseProgressDto> Handle(GetExerciseProgressQuery request, CancellationToken ct)
        => await _workoutReadService.GetExerciseProgressAsync(_currentUser.UserId, request.ExerciseName, ct);
}
