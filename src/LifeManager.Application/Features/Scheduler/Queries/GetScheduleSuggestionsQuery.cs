using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Scheduler.DTOs;
using MediatR;

namespace LifeManager.Application.Features.Scheduler.Queries;

public record GetScheduleSuggestionsQuery : IRequest<IReadOnlyList<ScheduleSuggestionDto>>;

public class GetScheduleSuggestionsQueryHandler
    : IRequestHandler<GetScheduleSuggestionsQuery, IReadOnlyList<ScheduleSuggestionDto>>
{
    private readonly ISchedulerService _scheduler;
    private readonly ICurrentUserService _currentUser;

    public GetScheduleSuggestionsQueryHandler(ISchedulerService scheduler, ICurrentUserService currentUser)
    {
        _scheduler = scheduler;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<ScheduleSuggestionDto>> Handle(
        GetScheduleSuggestionsQuery request, CancellationToken ct)
        => await _scheduler.GetSuggestionsAsync(_currentUser.UserId, ct);
}
