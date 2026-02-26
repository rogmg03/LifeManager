using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.TimeTracking.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.TimeTracking.Commands;

public record StartTimerCommand(Guid TaskId, string? Notes) : IRequest<TimeEntryDto>, IBaseCommand;

public class StartTimerCommandHandler : IRequestHandler<StartTimerCommand, TimeEntryDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public StartTimerCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<TimeEntryDto> Handle(StartTimerCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId;

        // Fail if another timer is already running for this user
        var existing = await _uow.TimeEntries.GetActiveTimerByUserIdAsync(userId, ct);
        if (existing is not null)
            throw new ConflictException(
                "A timer is already running. Stop the current timer before starting a new one.");

        var task = await _uow.Tasks.GetByIdAsync(request.TaskId, ct)
            ?? throw new NotFoundException("Task", request.TaskId);

        // Load the project for the DTO
        var project = await _uow.Projects.GetByIdAsync(task.ProjectId, ct);

        var entry = new TimeEntry
        {
            TaskId = request.TaskId,
            UserId = userId,
            StartedAt = DateTime.UtcNow,
            Notes = request.Notes,
            IsManual = false
        };

        await _uow.TimeEntries.AddAsync(entry, ct);
        await _uow.SaveChangesAsync(ct);

        return TimeEntryDto.FromEntityWithTask(
            entry,
            task.Title,
            task.ProjectId,
            project?.Name ?? string.Empty);
    }
}
