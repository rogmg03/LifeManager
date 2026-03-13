using Dapper;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Routines.DTOs;
using LifeManager.Application.Features.Workouts.DTOs;

namespace LifeManager.Infrastructure.Services;

public class WorkoutReadService : IWorkoutReadService
{
    private readonly IDbConnectionFactory _connectionFactory;
    public WorkoutReadService(IDbConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

    public async Task<List<RoutineDto>> GetRoutinesAsync(Guid userId, bool includeArchived, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT r.""Id"", r.""Name"", r.""Description"", r.""EstimatedDurationMinutes"",
                   r.""Category"", r.""IsArchived"", r.""SortOrder"",
                   r.""CreatedAt"", r.""UpdatedAt"",
                   COUNT(DISTINCT ri.""Id"")::int AS ""ItemCount"",
                   MAX(ws.""StartedAt"") AS ""LastWorkoutDate""
            FROM ""Routines"" r
            LEFT JOIN ""RoutineItems"" ri ON ri.""RoutineId"" = r.""Id""
            LEFT JOIN ""WorkoutSessions"" ws ON ws.""RoutineId"" = r.""Id"" AND ws.""UserId"" = @UserId
            WHERE r.""UserId"" = @UserId
              AND (@IncludeArchived = true OR r.""IsArchived"" = false)
            GROUP BY r.""Id"", r.""Name"", r.""Description"", r.""EstimatedDurationMinutes"",
                     r.""Category"", r.""IsArchived"", r.""SortOrder"",
                     r.""CreatedAt"", r.""UpdatedAt""
            ORDER BY r.""SortOrder"", r.""CreatedAt""";

        var rows = await conn.QueryAsync(new CommandDefinition(sql,
            new { UserId = userId, IncludeArchived = includeArchived },
            cancellationToken: ct));

        return rows.Select(r => new RoutineDto(
            (Guid)r.Id,
            (string)r.Name,
            (string?)r.Description,
            (int?)r.EstimatedDurationMinutes,
            (string?)r.Category,
            (bool)r.IsArchived,
            (int)r.SortOrder,
            (int)r.ItemCount,
            (DateTime?)r.LastWorkoutDate,
            (DateTime)r.CreatedAt,
            (DateTime)r.UpdatedAt
        )).ToList();
    }

    public async Task<WorkoutStatsDto> GetWorkoutStatsAsync(Guid userId, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();

        var now = DateTime.UtcNow;
        var weekStart = now.Date.AddDays(-(int)now.DayOfWeek);
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        const string statsSql = @"
            SELECT
                COUNT(*)::int AS ""Total"",
                COALESCE(AVG(""CompletionRate""), 0)::numeric(5,2) AS ""AvgRate"",
                COUNT(CASE WHEN ""StartedAt"" >= @WeekStart THEN 1 END)::int AS ""ThisWeek"",
                COUNT(CASE WHEN ""StartedAt"" >= @MonthStart THEN 1 END)::int AS ""ThisMonth""
            FROM ""WorkoutSessions""
            WHERE ""UserId"" = @UserId";

        var stats = await conn.QueryFirstAsync(new CommandDefinition(statsSql,
            new { UserId = userId, WeekStart = weekStart, MonthStart = monthStart },
            cancellationToken: ct));

        const string datesSql = @"
            SELECT DISTINCT DATE_TRUNC('day', ""StartedAt"")::timestamp AS ""Day""
            FROM ""WorkoutSessions""
            WHERE ""UserId"" = @UserId AND ""CompletedAt"" IS NOT NULL
            ORDER BY ""Day"" DESC";

        var sessionDates = (await conn.QueryAsync<DateTime>(new CommandDefinition(datesSql,
            new { UserId = userId }, cancellationToken: ct))).ToList();

        int streak = 0;
        var checkDate = DateTime.UtcNow.Date;

        foreach (var date in sessionDates)
        {
            if (date.Date == checkDate || date.Date == checkDate.AddDays(-1))
            {
                streak++;
                checkDate = date.Date.AddDays(-1);
            }
            else if (date.Date < checkDate)
            {
                break;
            }
        }

        return new WorkoutStatsDto(
            (int)stats.Total,
            (decimal)stats.AvgRate,
            streak,
            (int)stats.ThisWeek,
            (int)stats.ThisMonth);
    }

    public async Task<ExerciseProgressDto> GetExerciseProgressAsync(Guid userId, string exerciseName, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();

        const string historySql = @"
            SELECT
                DATE_TRUNC('day', ws.""StartedAt"")::date AS ""Date"",
                MAX(COALESCE(wset.""ActualWeight"", wset.""TargetWeight"", 0)) AS ""MaxWeight"",
                SUM(COALESCE(wset.""ActualReps"", wset.""TargetReps"", 0) *
                    COALESCE(wset.""ActualWeight"", wset.""TargetWeight"", 0)) AS ""TotalVolume"",
                MAX(COALESCE(wset.""ActualReps"", wset.""TargetReps"", 0)) AS ""BestSetReps""
            FROM ""WorkoutSets"" wset
            JOIN ""WorkoutSessions"" ws ON ws.""Id"" = wset.""SessionId""
            WHERE ws.""UserId"" = @UserId
              AND wset.""ExerciseName"" ILIKE @ExerciseName
              AND wset.""IsCompleted"" = true
            GROUP BY DATE_TRUNC('day', ws.""StartedAt"")
            ORDER BY ""Date"" ASC";

        var history = (await conn.QueryAsync<(DateTime Date, decimal MaxWeight, decimal TotalVolume, int BestSetReps)>(
            new CommandDefinition(historySql, new { UserId = userId, ExerciseName = exerciseName },
                cancellationToken: ct))).ToList();

        decimal? maxWeight = history.Count > 0 ? history.Max(h => h.MaxWeight) : null;
        decimal? avgReps = history.Count > 0 ? (decimal?)history.Average(h => h.BestSetReps) : null;

        decimal? estimated1RM = null;
        if (history.Count > 0)
        {
            var best = history.OrderByDescending(h => h.MaxWeight).First();
            if (best.MaxWeight > 0 && best.BestSetReps > 0)
                estimated1RM = Math.Round(best.MaxWeight * (1 + best.BestSetReps / 30m), 1);
        }

        var historyPoints = history
            .Select(h => new ExerciseHistoryPointDto(h.Date, h.MaxWeight, h.TotalVolume, h.BestSetReps))
            .ToList();

        return new ExerciseProgressDto(exerciseName, maxWeight, avgReps, estimated1RM, historyPoints);
    }
}
