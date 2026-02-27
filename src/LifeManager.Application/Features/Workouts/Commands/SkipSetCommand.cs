using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Workouts.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Workouts.Commands;

public record SkipSetCommand(Guid SetId) : IRequest<WorkoutSetDto>, IBaseCommand;

public class SkipSetCommandHandler : IRequestHandler<SkipSetCommand, WorkoutSetDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public SkipSetCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<WorkoutSetDto> Handle(SkipSetCommand request, CancellationToken ct)
    {
        var set = await _uow.WorkoutSets.GetByIdAsync(request.SetId, ct)
            ?? throw new NotFoundException("WorkoutSet", request.SetId);

        var session = await _uow.WorkoutSessions.GetByIdAsync(set.SessionId, ct)
            ?? throw new NotFoundException("WorkoutSession", set.SessionId);

        if (session.UserId != _currentUser.UserId)
            throw new NotFoundException("WorkoutSet", request.SetId);

        // If was previously completed, undo completion tracking
        if (set.IsCompleted)
        {
            session.CompletedSets = Math.Max(0, session.CompletedSets - 1);
            if (session.TotalSets > 0)
                session.CompletionRate = Math.Round((decimal)session.CompletedSets / session.TotalSets * 100, 2);
            _uow.WorkoutSessions.Update(session);
        }

        set.IsCompleted = false;
        set.CompletedAt = null;

        _uow.WorkoutSets.Update(set);
        await _uow.SaveChangesAsync(ct);

        return WorkoutSetDto.FromEntity(set);
    }
}
