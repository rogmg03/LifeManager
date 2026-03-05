using Dapper;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Dashboard.DTOs;

namespace LifeManager.Infrastructure.Services;

public class DashboardReadService : IDashboardReadService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DashboardReadService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync(Guid userId, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();

        var todayStart = DateTime.UtcNow.Date;
        var tomorrowStart = todayStart.AddDays(1);
        var param = new { UserId = userId, TodayStart = todayStart, TomorrowStart = tomorrowStart };

        const string sql = @"
            SELECT COUNT(*)::int FROM ""Projects""
            WHERE ""UserId"" = @UserId AND ""Status"" = 'Active';

            SELECT COUNT(*)::int
            FROM ""Tasks"" pt
            INNER JOIN ""Projects"" p ON p.""Id"" = pt.""ProjectId""
            WHERE p.""UserId"" = @UserId
              AND pt.""CompletedAt"" >= @TodayStart
              AND pt.""CompletedAt"" < @TomorrowStart;

            SELECT COUNT(*)::int
            FROM ""Tasks"" pt
            INNER JOIN ""Projects"" p ON p.""Id"" = pt.""ProjectId""
            WHERE p.""UserId"" = @UserId
              AND pt.""DueDate"" >= @TodayStart
              AND pt.""DueDate"" < @TomorrowStart
              AND pt.""Status"" != 'Done';

            SELECT COALESCE(SUM(""MinutesDelta""), 0)::int
            FROM ""FreeTimeTransactions""
            WHERE ""UserId"" = @UserId;

            SELECT p.""Name""
            FROM ""TimeEntries"" te
            INNER JOIN ""Tasks"" pt ON pt.""Id"" = te.""TaskId""
            INNER JOIN ""Projects"" p ON p.""Id"" = pt.""ProjectId""
            WHERE te.""UserId"" = @UserId AND te.""EndedAt"" IS NULL
            LIMIT 1;";

        var cmdDef = new CommandDefinition(sql, param, cancellationToken: ct);
        using var multi = await conn.QueryMultipleAsync(cmdDef);

        var activeProjectsCount = await multi.ReadFirstAsync<int>();
        var tasksCompletedToday = await multi.ReadFirstAsync<int>();
        var tasksDueToday = await multi.ReadFirstAsync<int>();
        var freeTimeBalance = await multi.ReadFirstAsync<int>();
        var activeTimerProjectName = await multi.ReadFirstOrDefaultAsync<string?>();

        return new DashboardSummaryDto
        {
            ActiveProjectsCount = activeProjectsCount,
            TasksCompletedToday = tasksCompletedToday,
            TasksDueToday = tasksDueToday,
            FreeTimeBalanceMinutes = freeTimeBalance,
            ActiveTimerProjectName = activeTimerProjectName
        };
    }

    public async Task<IEnumerable<ScheduleBlockItemDto>> GetTodaysScheduleAsync(Guid userId, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();

        var todayStart = DateTime.UtcNow.Date;
        var tomorrowStart = todayStart.AddDays(1);

        const string sql = @"
            SELECT ""Id"", ""Title"", ""BlockType"", ""StartTime"", ""EndTime"", ""Status""
            FROM ""ScheduleBlocks""
            WHERE ""UserId"" = @UserId
              AND ""StartTime"" >= @TodayStart
              AND ""StartTime"" < @TomorrowStart
            ORDER BY ""StartTime"" ASC;";

        var cmdDef = new CommandDefinition(sql, new { UserId = userId, TodayStart = todayStart, TomorrowStart = tomorrowStart }, cancellationToken: ct);
        return await conn.QueryAsync<ScheduleBlockItemDto>(cmdDef);
    }

    public async Task<IEnumerable<UpcomingTaskDto>> GetUpcomingTasksAsync(Guid userId, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();

        var todayStart = DateTime.UtcNow.Date;

        const string sql = @"
            SELECT pt.""Id"", pt.""Title"", p.""Name"" AS ""ProjectName"", pt.""DueDate"", pt.""Priority""
            FROM ""Tasks"" pt
            INNER JOIN ""Projects"" p ON p.""Id"" = pt.""ProjectId""
            WHERE p.""UserId"" = @UserId
              AND pt.""DueDate"" >= @TodayStart
              AND pt.""Status"" != 'Done'
            ORDER BY pt.""DueDate"" ASC
            LIMIT 10;";

        var cmdDef = new CommandDefinition(sql, new { UserId = userId, TodayStart = todayStart }, cancellationToken: ct);
        return await conn.QueryAsync<UpcomingTaskDto>(cmdDef);
    }

    public async Task<IEnumerable<RecentActivityDto>> GetRecentActivityAsync(Guid userId, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT ae.""Id"", ae.""Description"", ae.""CreatedAt"" AS ""OccurredAt"", p.""Name"" AS ""ProjectName""
            FROM ""ActivityEntries"" ae
            LEFT JOIN ""Projects"" p ON p.""Id"" = ae.""ProjectId""
            WHERE ae.""UserId"" = @UserId
            ORDER BY ae.""CreatedAt"" DESC
            LIMIT 20;";

        var cmdDef = new CommandDefinition(sql, new { UserId = userId }, cancellationToken: ct);
        return await conn.QueryAsync<RecentActivityDto>(cmdDef);
    }
}
