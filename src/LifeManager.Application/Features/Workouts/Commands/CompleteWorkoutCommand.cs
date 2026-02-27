using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Workouts.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Workouts.Commands;

public record CompleteWorkoutCommand(Guid Id, string? Notes) : IRequest<WorkoutSessionDto>, IBaseCommand;

public class CompleteWorkoutCommandHandler : IRequestHandler<CompleteWorkoutCommand, WorkoutSessionDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CompleteWorkoutCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<WorkoutSessionDto> Handle(CompleteWorkoutCommand request, CancellationToken ct)
    {
        var session = await _uow.WorkoutSessions.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("WorkoutSession", request.Id);

        if (session.UserId != _currentUser.UserId)
            throw new NotFoundException("WorkoutSession", request.Id);

        session.CompletedAt = DateTimeOffset.UtcNow;
        session.DurationSeconds = (int)(session.CompletedAt.Value - session.StartedAt).TotalSeconds;
        session.Notes = request.Notes;

        if (session.TotalSets > 0)
            session.CompletionRate = Math.Round((decimal)session.CompletedSets / session.TotalSets * 100, 2);

        _uow.WorkoutSessions.Update(session);
        await _uow.SaveChangesAsync(ct);

        return WorkoutSessionDto.FromEntity(session);
    }
}
