using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Workouts.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Workouts.Queries;

public record GetWorkoutByIdQuery(Guid Id) : IRequest<WorkoutSessionDetailDto>;

public class GetWorkoutByIdQueryHandler : IRequestHandler<GetWorkoutByIdQuery, WorkoutSessionDetailDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetWorkoutByIdQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<WorkoutSessionDetailDto> Handle(GetWorkoutByIdQuery request, CancellationToken ct)
    {
        var session = await _uow.WorkoutSessions.GetByIdWithSetsAsync(request.Id, ct)
            ?? throw new NotFoundException("WorkoutSession", request.Id);

        if (session.UserId != _currentUser.UserId)
            throw new NotFoundException("WorkoutSession", request.Id);

        // Group sets by exercise name / routine item
        var exercises = session.Sets
            .GroupBy(s => new { s.ExerciseName, s.RoutineItemId })
            .OrderBy(g => session.Sets.First(s => s.ExerciseName == g.Key.ExerciseName).SetNumber)
            .Select(g =>
            {
                var firstSet = g.OrderBy(s => s.SetNumber).First();
                return new WorkoutExerciseGroupDto(
                    g.Key.ExerciseName,
                    g.Key.RoutineItemId,
                    g.Count(),
                    firstSet.TargetReps,
                    firstSet.TargetWeight,
                    60, // default rest — RoutineItem data not loaded here
                    g.OrderBy(s => s.SetNumber).Select(WorkoutSetDto.FromEntity).ToList());
            }).ToList();

        return new WorkoutSessionDetailDto(
            session.Id, session.RoutineId, session.RoutineName,
            session.StartedAt, session.CompletedAt, session.DurationSeconds,
            session.TotalSets, session.CompletedSets, session.CompletionRate,
            session.Notes, exercises);
    }
}
