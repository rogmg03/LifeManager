using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Routines.DTOs;
using MediatR;

namespace LifeManager.Application.Features.Routines.Queries;

public record GetRoutinesQuery(bool IncludeArchived = false) : IRequest<List<RoutineDto>>;

public class GetRoutinesQueryHandler : IRequestHandler<GetRoutinesQuery, List<RoutineDto>>
{
    private readonly IWorkoutReadService _workoutReadService;
    private readonly ICurrentUserService _currentUser;

    public GetRoutinesQueryHandler(IWorkoutReadService workoutReadService, ICurrentUserService currentUser)
    {
        _workoutReadService = workoutReadService;
        _currentUser = currentUser;
    }

    public async Task<List<RoutineDto>> Handle(GetRoutinesQuery request, CancellationToken ct)
        => await _workoutReadService.GetRoutinesAsync(_currentUser.UserId, request.IncludeArchived, ct);
}
