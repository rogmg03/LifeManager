using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Notifications.Commands;

public record DeleteNotificationCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteNotificationCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteNotificationCommand request, CancellationToken ct)
    {
        var notification = await _uow.NotificationLogs.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Notification", request.Id);

        _uow.NotificationLogs.Delete(notification);
        await _uow.SaveChangesAsync(ct);
    }
}
