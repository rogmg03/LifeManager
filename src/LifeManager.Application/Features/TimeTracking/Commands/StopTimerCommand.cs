using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.TimeTracking.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Events;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.TimeTracking.Commands;

public record StopTimerCommand(Guid TaskId) : IRequest<TimeEntryDto>, IBaseCommand;

public class StopTimerCommandHandler : IRequestHandler<StopTimerCommand, TimeEntryDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public StopTimerCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<TimeEntryDto> Handle(StopTimerCommand request, CancellationToken ct)
    {
        var entry = await _uow.TimeEntries.GetRunningByTaskIdAsync(request.TaskId, ct)
            ?? throw new NotFoundException("No running timer found for this task.");

        var now = DateTime.UtcNow;
        entry.EndedAt = now;
        entry.DurationMinutes = (int)Math.Ceiling((now - entry.StartedAt).TotalMinutes);

        // Domain event — Cycle 10 handler will create FreeTimeTransaction
        entry.AddDomainEvent(new TimeEntryCompletedEvent(
            entry.Id,
            entry.UserId,
            entry.TaskId,
            entry.Task.Title,
            entry.DurationMinutes.Value));

        // Activity entry for the feed
        var activity = new ActivityEntry
        {
            UserId = entry.UserId,
            ProjectId = entry.Task.ProjectId,
            ActivityType = ActivityType.TimeLogged,
            Description = $"Logged {entry.DurationMinutes} min on \"{entry.Task.Title}\"",
            EntityId = entry.Id,
            EntityType = "TimeEntry"
        };

        _uow.TimeEntries.Update(entry);
        await _uow.ActivityEntries.AddAsync(activity, ct);
        await _uow.SaveChangesAsync(ct);

        return TimeEntryDto.FromEntity(entry);
    }
}
