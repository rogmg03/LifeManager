using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Schedule.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Schedule.Queries;

public record GetMonthlyScheduleQuery(int Year, int Month) : IRequest<List<ScheduleBlockDto>>;

public class GetMonthlyScheduleQueryHandler : IRequestHandler<GetMonthlyScheduleQuery, List<ScheduleBlockDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetMonthlyScheduleQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<List<ScheduleBlockDto>> Handle(GetMonthlyScheduleQuery request, CancellationToken ct)
    {
        var from = new DateTime(request.Year, request.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var to = from.AddMonths(1);

        var blocks = await _uow.ScheduleBlocks.GetByDateRangeAsync(_currentUser.UserId, from, to, ct);
        return blocks.Select(ScheduleBlockDto.FromEntity).ToList();
    }
}
