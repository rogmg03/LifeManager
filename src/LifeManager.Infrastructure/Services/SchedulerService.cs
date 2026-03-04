using Dapper;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Scheduler.DTOs;

namespace LifeManager.Infrastructure.Services;

public class SchedulerService : ISchedulerService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SchedulerService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ScheduleSuggestionDto>> GetUnscheduledTasksAsync(
        Guid userId, DateTime dayStart, DateTime dayEnd, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT
                pt.""Id""             AS ""TaskId"",
                pt.""Title"",
                p.""Id""              AS ""ProjectId"",
                p.""Name""            AS ""ProjectName"",
                pt.""DueDate"",
                pt.""Priority"",
                pt.""EstimatedMinutes""
            FROM ""ProjectTasks"" pt
            INNER JOIN ""Projects"" p ON p.""Id"" = pt.""ProjectId""
            WHERE p.""UserId"" = @UserId
              AND pt.""Status"" != 'Done'
              AND pt.""EstimatedMinutes"" IS NOT NULL
              AND pt.""EstimatedMinutes"" > 0
              AND NOT EXISTS (
                  SELECT 1 FROM ""ScheduleBlocks"" sb
                  WHERE sb.""TaskId"" = pt.""Id""
                    AND sb.""StartTime"" >= @DayStart
                    AND sb.""StartTime"" < @DayEnd
                    AND sb.""Status"" != 'Skipped'
              )
            ORDER BY
                CASE WHEN pt.""DueDate"" < @DayStart THEN 0
                     WHEN pt.""DueDate"" >= @DayStart AND pt.""DueDate"" < @DayEnd THEN 1
                     ELSE 2 END,
                CASE pt.""Priority""
                    WHEN 'Urgent' THEN 0
                    WHEN 'High'   THEN 1
                    WHEN 'Medium' THEN 2
                    ELSE 3 END,
                pt.""DueDate"" ASC NULLS LAST;";

        var cmdDef = new CommandDefinition(sql,
            new { UserId = userId, DayStart = dayStart, DayEnd = dayEnd },
            cancellationToken: ct);

        var rows = await conn.QueryAsync<ScheduleSuggestionDto>(cmdDef);
        return rows.ToList();
    }

    public async Task<IReadOnlyList<ScheduleSuggestionDto>> GetSuggestionsAsync(
        Guid userId, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();

        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        const string sql = @"
            SELECT
                pt.""Id""             AS ""TaskId"",
                pt.""Title"",
                p.""Id""              AS ""ProjectId"",
                p.""Name""            AS ""ProjectName"",
                pt.""DueDate"",
                pt.""Priority"",
                pt.""EstimatedMinutes""
            FROM ""ProjectTasks"" pt
            INNER JOIN ""Projects"" p ON p.""Id"" = pt.""ProjectId""
            WHERE p.""UserId"" = @UserId
              AND pt.""Status"" != 'Done'
            ORDER BY
                CASE WHEN pt.""DueDate"" < @Today    THEN 0
                     WHEN pt.""DueDate"" >= @Today
                      AND pt.""DueDate"" <  @Tomorrow THEN 1
                     ELSE 2 END,
                CASE pt.""Priority""
                    WHEN 'Urgent' THEN 0
                    WHEN 'High'   THEN 1
                    WHEN 'Medium' THEN 2
                    ELSE 3 END,
                pt.""DueDate"" ASC NULLS LAST
            LIMIT 10;";

        var cmdDef = new CommandDefinition(sql,
            new { UserId = userId, Today = today, Tomorrow = tomorrow },
            cancellationToken: ct);

        var rows = await conn.QueryAsync<ScheduleSuggestionDto>(cmdDef);
        return rows.ToList();
    }
}
