using LifeManager.Domain.Entities;

namespace LifeManager.Application.Features.Notifications.DTOs;

public record NotificationLogDto(
    Guid Id,
    Guid UserId,
    string Channel,
    string Message,
    bool IsRead,
    DateTime CreatedAt)
{
    public static NotificationLogDto FromEntity(NotificationLog n) => new(
        n.Id, n.UserId, n.Channel.ToString(), n.Message, n.IsRead, n.CreatedAt);
}
