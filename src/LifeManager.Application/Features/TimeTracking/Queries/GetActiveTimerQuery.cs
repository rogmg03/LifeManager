using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.TimeTracking.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.TimeTracking.Queries;

public record GetActiveTimerQuery : IRequest<TimeEntryDto?>;

public class GetActiveTimerQueryHandler : IRequestHandler<GetActiveTimerQuery, TimeEntryDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetActiveTimerQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<TimeEntryDto?> Handle(GetActiveTimerQuery request, CancellationToken ct)
    {
        var entry = await _uow.TimeEntries.GetActiveTimerByUserIdAsync(_currentUser.UserId, ct);
        return entry is null ? null : TimeEntryDto.FromEntity(entry);
    }
}
