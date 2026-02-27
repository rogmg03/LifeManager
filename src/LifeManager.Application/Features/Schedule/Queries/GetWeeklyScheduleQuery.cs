using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Schedule.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Schedule.Queries;

public record GetWeeklyScheduleQuery(DateOnly StartDate) : IRequest<List<ScheduleBlockDto>>;

public class GetWeeklyScheduleQueryHandler : IRequestHandler<GetWeeklyScheduleQuery, List<ScheduleBlockDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetWeeklyScheduleQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<List<ScheduleBlockDto>> Handle(GetWeeklyScheduleQuery request, CancellationToken ct)
    {
        var from = request.StartDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var to = from.AddDays(7);

        var blocks = await _uow.ScheduleBlocks.GetByDateRangeAsync(_currentUser.UserId, from, to, ct);
        return blocks.Select(ScheduleBlockDto.FromEntity).ToList();
    }
}
