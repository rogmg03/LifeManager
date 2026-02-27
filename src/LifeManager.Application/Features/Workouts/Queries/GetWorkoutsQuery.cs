using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Common.Models;
using LifeManager.Application.Features.Workouts.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Workouts.Queries;

public record GetWorkoutsQuery(
    Guid? RoutineId = null,
    DateTimeOffset? From = null,
    DateTimeOffset? To = null,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<WorkoutSessionDto>>;

public class GetWorkoutsQueryHandler : IRequestHandler<GetWorkoutsQuery, PagedResult<WorkoutSessionDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetWorkoutsQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<PagedResult<WorkoutSessionDto>> Handle(GetWorkoutsQuery request, CancellationToken ct)
    {
        var sessions = await _uow.WorkoutSessions.GetByUserIdAsync(
            _currentUser.UserId, request.RoutineId, request.From, request.To,
            request.Page, request.PageSize, ct);

        // For total count, get all (no paging) — simple approach for now
        var allSessions = await _uow.WorkoutSessions.GetByUserIdAsync(
            _currentUser.UserId, request.RoutineId, request.From, request.To,
            1, int.MaxValue, ct);

        var items = sessions.Select(WorkoutSessionDto.FromEntity).ToList();
        return new PagedResult<WorkoutSessionDto>(items, allSessions.Count, request.Page, request.PageSize);
    }
}
