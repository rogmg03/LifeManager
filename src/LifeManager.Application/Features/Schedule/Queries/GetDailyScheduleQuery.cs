using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Schedule.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Schedule.Queries;

public record GetDailyScheduleQuery(DateOnly Date) : IRequest<List<ScheduleBlockDto>>;

public class GetDailyScheduleQueryHandler : IRequestHandler<GetDailyScheduleQuery, List<ScheduleBlockDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetDailyScheduleQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<List<ScheduleBlockDto>> Handle(GetDailyScheduleQuery request, CancellationToken ct)
    {
        var from = request.Date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var to = from.AddDays(1);

        var blocks = await _uow.ScheduleBlocks.GetByDateRangeAsync(_currentUser.UserId, from, to, ct);
        return blocks.Select(ScheduleBlockDto.FromEntity).ToList();
    }
}
