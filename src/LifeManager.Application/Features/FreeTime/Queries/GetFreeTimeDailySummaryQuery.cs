using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.FreeTime.DTOs;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.FreeTime.Queries;

public record GetFreeTimeDailySummaryQuery(DateTime Date) : IRequest<FreeTimeDailySummaryDto>;

public class GetFreeTimeDailySummaryQueryHandler
    : IRequestHandler<GetFreeTimeDailySummaryQuery, FreeTimeDailySummaryDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetFreeTimeDailySummaryQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<FreeTimeDailySummaryDto> Handle(
        GetFreeTimeDailySummaryQuery request, CancellationToken ct)
    {
        var userId = _currentUser.UserId;

        // Date window in UTC (treat the date param as a calendar day)
        var dayStart = DateTime.SpecifyKind(request.Date.Date, DateTimeKind.Utc);
        var dayEnd = dayStart.AddDays(1);

        // Transactions for the day
        var (transactions, _) = await _uow.FreeTimeTransactions.GetPagedByUserIdAsync(
            userId, dayStart, dayEnd, page: 1, pageSize: int.MaxValue, ct);

        var earnedMinutes = transactions
            .Where(t => t.Type == TransactionType.Earned)
            .Sum(t => t.MinutesDelta);

        var spentMinutes = transactions
            .Where(t => t.Type == TransactionType.Spent)
            .Sum(t => Math.Abs(t.MinutesDelta));

        // Balance at end of day = last transaction's BalanceAfterMinutes (or current balance if none)
        var endOfDayBalance = transactions.Count > 0
            ? transactions.Last().BalanceAfterMinutes
            : await _uow.FreeTimeTransactions.GetLatestBalanceForUserAsync(userId, ct);

        // Worked minutes — sum of completed TimeEntries that ended on this day
        var timeEntries = await _uow.TimeEntries.GetByDateRangeAsync(userId, dayStart, dayEnd, ct);
        var workedMinutes = timeEntries
            .Where(te => te.DurationMinutes.HasValue)
            .Sum(te => te.DurationMinutes!.Value);

        return new FreeTimeDailySummaryDto(
            Date: request.Date.Date,
            WorkedMinutes: workedMinutes,
            EarnedMinutes: earnedMinutes,
            SpentMinutes: spentMinutes,
            BalanceAtEndOfDay: endOfDayBalance);
    }
}
