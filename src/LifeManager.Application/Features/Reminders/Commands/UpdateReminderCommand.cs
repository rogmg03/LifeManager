using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Reminders.DTOs;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Reminders.Commands;

public record UpdateReminderCommand(
    Guid Id,
    string Title,
    ReminderType ReminderType,
    DateTime RemindAt,
    Guid? TaskId,
    Guid? ScheduleBlockId,
    string? Notes) : IRequest<ReminderDto>, IBaseCommand;

public class UpdateReminderCommandHandler : IRequestHandler<UpdateReminderCommand, ReminderDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateReminderCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ReminderDto> Handle(UpdateReminderCommand request, CancellationToken ct)
    {
        var reminder = await _uow.Reminders.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Reminder", request.Id);

        reminder.Title = request.Title;
        reminder.ReminderType = request.ReminderType;
        reminder.RemindAt = request.RemindAt;
        reminder.TaskId = request.TaskId;
        reminder.ScheduleBlockId = request.ScheduleBlockId;
        reminder.Notes = request.Notes;

        _uow.Reminders.Update(reminder);
        await _uow.SaveChangesAsync(ct);
        return ReminderDto.FromEntity(reminder);
    }
}
