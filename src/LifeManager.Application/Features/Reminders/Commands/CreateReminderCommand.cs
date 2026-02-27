using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Reminders.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Reminders.Commands;

public record CreateReminderCommand(
    string Title,
    ReminderType ReminderType,
    DateTime RemindAt,
    Guid? TaskId,
    Guid? ScheduleBlockId,
    string? Notes) : IRequest<ReminderDto>, IBaseCommand;

public class CreateReminderCommandHandler : IRequestHandler<CreateReminderCommand, ReminderDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CreateReminderCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<ReminderDto> Handle(CreateReminderCommand request, CancellationToken ct)
    {
        var reminder = new Reminder
        {
            UserId = _currentUser.UserId,
            Title = request.Title,
            ReminderType = request.ReminderType,
            RemindAt = request.RemindAt,
            TaskId = request.TaskId,
            ScheduleBlockId = request.ScheduleBlockId,
            Notes = request.Notes,
            Status = ReminderStatus.Pending
        };

        await _uow.Reminders.AddAsync(reminder, ct);
        await _uow.SaveChangesAsync(ct);
        return ReminderDto.FromEntity(reminder);
    }
}
