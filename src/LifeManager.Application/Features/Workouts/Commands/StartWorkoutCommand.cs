using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Workouts.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Workouts.Commands;

public record StartWorkoutCommand(Guid RoutineId) : IRequest<WorkoutSessionDetailDto>, IBaseCommand;

public class StartWorkoutCommandHandler : IRequestHandler<StartWorkoutCommand, WorkoutSessionDetailDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public StartWorkoutCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<WorkoutSessionDetailDto> Handle(StartWorkoutCommand request, CancellationToken ct)
    {
        var routine = await _uow.Routines.GetByIdWithItemsAsync(request.RoutineId, ct)
            ?? throw new NotFoundException("Routine", request.RoutineId);

        if (routine.UserId != _currentUser.UserId)
            throw new NotFoundException("Routine", request.RoutineId);

        var totalSets = routine.Items.Sum(i => i.TargetSets);

        var session = new WorkoutSession
        {
            UserId = _currentUser.UserId,
            RoutineId = routine.Id,
            RoutineName = routine.Name,
            StartedAt = DateTimeOffset.UtcNow,
            TotalSets = totalSets,
            CompletedSets = 0,
            CompletionRate = 0
        };

        await _uow.WorkoutSessions.AddAsync(session, ct);
        await _uow.SaveChangesAsync(ct);

        // Pre-generate all WorkoutSet rows
        var sets = new List<WorkoutSet>();
        foreach (var item in routine.Items.OrderBy(i => i.SortOrder))
        {
            for (int setNum = 1; setNum <= item.TargetSets; setNum++)
            {
                sets.Add(new WorkoutSet
                {
                    SessionId = session.Id,
                    RoutineItemId = item.Id,
                    ExerciseName = item.ExerciseName,
                    SetNumber = setNum,
                    TargetReps = item.TargetReps,
                    TargetWeight = item.TargetWeight,
                    IsCompleted = false
                });
            }
        }

        await _uow.WorkoutSets.AddRangeAsync(sets, ct);
        await _uow.SaveChangesAsync(ct);

        // Build grouped exercise view
        var exercises = routine.Items.OrderBy(i => i.SortOrder).Select(item =>
        {
            var itemSets = sets.Where(s => s.RoutineItemId == item.Id)
                               .OrderBy(s => s.SetNumber)
                               .Select(WorkoutSetDto.FromEntity)
                               .ToList();
            return new WorkoutExerciseGroupDto(
                item.ExerciseName, item.Id,
                item.TargetSets, item.TargetReps, item.TargetWeight, item.RestSeconds,
                itemSets);
        }).ToList();

        return new WorkoutSessionDetailDto(
            session.Id, session.RoutineId, session.RoutineName,
            session.StartedAt, session.CompletedAt, session.DurationSeconds,
            session.TotalSets, session.CompletedSets, session.CompletionRate,
            session.Notes, exercises);
    }
}
