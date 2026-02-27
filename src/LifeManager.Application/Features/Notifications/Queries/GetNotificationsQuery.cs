using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Common.Models;
using LifeManager.Application.Features.Notifications.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Notifications.Queries;

public record GetNotificationsQuery(int Page = 1, int PageSize = 20) : IRequest<PagedResult<NotificationLogDto>>;

public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, PagedResult<NotificationLogDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetNotificationsQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<PagedResult<NotificationLogDto>> Handle(GetNotificationsQuery request, CancellationToken ct)
    {
        var (items, total) = await _uow.NotificationLogs.GetByUserIdAsync(
            _currentUser.UserId, request.Page, request.PageSize, ct);

        return new PagedResult<NotificationLogDto>(
            items.Select(NotificationLogDto.FromEntity).ToList(),
            total,
            request.Page,
            request.PageSize);
    }
}
