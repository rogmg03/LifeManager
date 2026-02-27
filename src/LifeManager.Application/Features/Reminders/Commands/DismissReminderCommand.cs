using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Reminders.DTOs;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Reminders.Commands;

public record DismissReminderCommand(Guid Id) : IRequest<ReminderDto>, IBaseCommand;

public class DismissReminderCommandHandler : IRequestHandler<DismissReminderCommand, ReminderDto>
{
    private readonly IUnitOfWork _uow;
    public DismissReminderCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ReminderDto> Handle(DismissReminderCommand request, CancellationToken ct)
    {
        var reminder = await _uow.Reminders.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Reminder", request.Id);

        reminder.Status = ReminderStatus.Dismissed;
        _uow.Reminders.Update(reminder);
        await _uow.SaveChangesAsync(ct);
        return ReminderDto.FromEntity(reminder);
    }
}
