using LifeManager.Application.Common.Interfaces;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Notifications.Queries;

public record GetUnreadCountQuery : IRequest<int>;

public class GetUnreadCountQueryHandler : IRequestHandler<GetUnreadCountQuery, int>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetUnreadCountQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public Task<int> Handle(GetUnreadCountQuery request, CancellationToken ct)
        => _uow.NotificationLogs.GetUnreadCountAsync(_currentUser.UserId, ct);
}
