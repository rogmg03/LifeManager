using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.DailyGoals.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.DailyGoals.Queries;

public record GetDailyGoalsQuery : IRequest<IReadOnlyList<DailyGoalDto>>;

public class GetDailyGoalsQueryHandler : IRequestHandler<GetDailyGoalsQuery, IReadOnlyList<DailyGoalDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetDailyGoalsQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<DailyGoalDto>> Handle(GetDailyGoalsQuery request, CancellationToken ct)
    {
        var goals = await _uow.DailyGoals.GetByUserIdAsync(_currentUser.UserId, ct);
        return goals.Select(DailyGoalDto.FromEntity).ToList();
    }
}
