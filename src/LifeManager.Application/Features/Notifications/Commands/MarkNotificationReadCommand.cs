using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Notifications.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Notifications.Commands;

public record MarkNotificationReadCommand(Guid Id) : IRequest<NotificationLogDto>, IBaseCommand;

public class MarkNotificationReadCommandHandler : IRequestHandler<MarkNotificationReadCommand, NotificationLogDto>
{
    private readonly IUnitOfWork _uow;
    public MarkNotificationReadCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<NotificationLogDto> Handle(MarkNotificationReadCommand request, CancellationToken ct)
    {
        var notification = await _uow.NotificationLogs.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Notification", request.Id);

        notification.IsRead = true;
        _uow.NotificationLogs.Update(notification);
        await _uow.SaveChangesAsync(ct);
        return NotificationLogDto.FromEntity(notification);
    }
}
