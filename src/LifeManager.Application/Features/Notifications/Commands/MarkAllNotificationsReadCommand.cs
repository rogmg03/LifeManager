using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Notifications.Commands;

public record MarkAllNotificationsReadCommand : IRequest, IBaseCommand;

public class MarkAllNotificationsReadCommandHandler : IRequestHandler<MarkAllNotificationsReadCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public MarkAllNotificationsReadCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task Handle(MarkAllNotificationsReadCommand request, CancellationToken ct)
        => await _uow.NotificationLogs.MarkAllAsReadAsync(_currentUser.UserId, ct);
}
