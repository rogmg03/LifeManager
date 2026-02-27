using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Routines.DTOs;
using LifeManager.Application.Features.Workouts.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Routines.Queries;

public record GetRoutineByIdQuery(Guid Id) : IRequest<RoutineDetailDto>;

public class GetRoutineByIdQueryHandler : IRequestHandler<GetRoutineByIdQuery, RoutineDetailDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetRoutineByIdQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<RoutineDetailDto> Handle(GetRoutineByIdQuery request, CancellationToken ct)
    {
        var routine = await _uow.Routines.GetByIdWithItemsAsync(request.Id, ct)
            ?? throw new NotFoundException("Routine", request.Id);

        if (routine.UserId != _currentUser.UserId)
            throw new NotFoundException("Routine", request.Id);

        var recentSessions = await _uow.WorkoutSessions.GetByUserIdAsync(
            _currentUser.UserId, routineId: request.Id, pageSize: 5, ct: ct);

        var items = routine.Items.Select(RoutineItemDto.FromEntity).ToList();
        var sessions = recentSessions.Select(WorkoutSessionDto.FromEntity).ToList();

        DateTime? lastWorkoutDate = recentSessions.Count > 0
            ? recentSessions[0].StartedAt.DateTime
            : null;

        return new RoutineDetailDto(
            routine.Id, routine.Name, routine.Description,
            routine.EstimatedDurationMinutes, routine.Category, routine.IsArchived,
            routine.SortOrder, items.Count, lastWorkoutDate,
            routine.CreatedAt, routine.UpdatedAt, items, sessions);
    }
}
