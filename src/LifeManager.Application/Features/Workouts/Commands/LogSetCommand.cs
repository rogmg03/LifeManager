using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Workouts.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Workouts.Commands;

public record LogSetCommand(
    Guid SetId,
    int? ActualReps,
    decimal? ActualWeight,
    bool IsCompleted) : IRequest<WorkoutSetDto>, IBaseCommand;

public class LogSetCommandHandler : IRequestHandler<LogSetCommand, WorkoutSetDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public LogSetCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<WorkoutSetDto> Handle(LogSetCommand request, CancellationToken ct)
    {
        var set = await _uow.WorkoutSets.GetByIdAsync(request.SetId, ct)
            ?? throw new NotFoundException("WorkoutSet", request.SetId);

        var session = await _uow.WorkoutSessions.GetByIdAsync(set.SessionId, ct)
            ?? throw new NotFoundException("WorkoutSession", set.SessionId);

        if (session.UserId != _currentUser.UserId)
            throw new NotFoundException("WorkoutSet", request.SetId);

        var wasCompleted = set.IsCompleted;

        set.ActualReps = request.ActualReps;
        set.ActualWeight = request.ActualWeight;
        set.IsCompleted = request.IsCompleted;

        if (request.IsCompleted && !wasCompleted)
        {
            set.CompletedAt = DateTimeOffset.UtcNow;
            session.CompletedSets++;
        }
        else if (!request.IsCompleted && wasCompleted)
        {
            set.CompletedAt = null;
            session.CompletedSets = Math.Max(0, session.CompletedSets - 1);
        }

        if (session.TotalSets > 0)
            session.CompletionRate = Math.Round((decimal)session.CompletedSets / session.TotalSets * 100, 2);

        _uow.WorkoutSets.Update(set);
        _uow.WorkoutSessions.Update(session);
        await _uow.SaveChangesAsync(ct);

        return WorkoutSetDto.FromEntity(set);
    }
}
