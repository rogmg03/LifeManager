using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Reminders.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Reminders.Queries;

public record GetRemindersQuery(bool PendingOnly = false) : IRequest<List<ReminderDto>>;

public class GetRemindersQueryHandler : IRequestHandler<GetRemindersQuery, List<ReminderDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetRemindersQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<List<ReminderDto>> Handle(GetRemindersQuery request, CancellationToken ct)
    {
        var reminders = await _uow.Reminders.GetByUserIdAsync(
            _currentUser.UserId, request.PendingOnly, ct);
        return reminders.Select(ReminderDto.FromEntity).ToList();
    }
}
