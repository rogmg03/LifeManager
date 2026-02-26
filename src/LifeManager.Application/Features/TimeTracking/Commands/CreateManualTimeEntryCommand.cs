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

public record CreateManualTimeEntryCommand(
    Guid TaskId,
    DateTime StartedAt,
    DateTime EndedAt,
    string? Notes) : IRequest<TimeEntryDto>, IBaseCommand;

public class CreateManualTimeEntryCommandHandler : IRequestHandler<CreateManualTimeEntryCommand, TimeEntryDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CreateManualTimeEntryCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<TimeEntryDto> Handle(CreateManualTimeEntryCommand request, CancellationToken ct)
    {
        if (request.EndedAt <= request.StartedAt)
            throw new ConflictException("EndedAt must be after StartedAt.");

        var task = await _uow.Tasks.GetByIdAsync(request.TaskId, ct)
            ?? throw new NotFoundException("Task", request.TaskId);

        var project = await _uow.Projects.GetByIdAsync(task.ProjectId, ct);
        var durationMinutes = (int)Math.Ceiling((request.EndedAt - request.StartedAt).TotalMinutes);
        var userId = _currentUser.UserId;

        var entry = new TimeEntry
        {
            TaskId = request.TaskId,
            UserId = userId,
            StartedAt = request.StartedAt,
            EndedAt = request.EndedAt,
            DurationMinutes = durationMinutes,
            Notes = request.Notes,
            IsManual = true
        };

        // entry.Id is set by BaseEntity constructor — safe to reference before save
        entry.AddDomainEvent(new TimeEntryCompletedEvent(
            entry.Id, userId, entry.TaskId, task.Title, durationMinutes));

        var activity = new ActivityEntry
        {
            UserId = userId,
            ProjectId = task.ProjectId,
            ActivityType = ActivityType.TimeLogged,
            Description = $"Manually logged {durationMinutes} min on \"{task.Title}\"",
            EntityId = entry.Id,
            EntityType = "TimeEntry"
        };

        await _uow.TimeEntries.AddAsync(entry, ct);
        await _uow.ActivityEntries.AddAsync(activity, ct);
        await _uow.SaveChangesAsync(ct);

        return TimeEntryDto.FromEntityWithTask(
            entry, task.Title, task.ProjectId, project?.Name ?? string.Empty);
    }
}
