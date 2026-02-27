using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Reminders.Commands;

public record DeleteReminderCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteReminderCommandHandler : IRequestHandler<DeleteReminderCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteReminderCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteReminderCommand request, CancellationToken ct)
    {
        var reminder = await _uow.Reminders.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Reminder", request.Id);

        _uow.Reminders.Delete(reminder);
        await _uow.SaveChangesAsync(ct);
    }
}
