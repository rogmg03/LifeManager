using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Reminders.DTOs;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Reminders.Commands;

public record SnoozeReminderCommand(Guid Id, DateTime SnoozedUntil) : IRequest<ReminderDto>, IBaseCommand;

public class SnoozeReminderCommandHandler : IRequestHandler<SnoozeReminderCommand, ReminderDto>
{
    private readonly IUnitOfWork _uow;
    public SnoozeReminderCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ReminderDto> Handle(SnoozeReminderCommand request, CancellationToken ct)
    {
        var reminder = await _uow.Reminders.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Reminder", request.Id);

        reminder.RemindAt = request.SnoozedUntil;
        reminder.Status = ReminderStatus.Pending;
        _uow.Reminders.Update(reminder);
        await _uow.SaveChangesAsync(ct);
        return ReminderDto.FromEntity(reminder);
    }
}
