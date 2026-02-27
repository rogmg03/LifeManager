using Dapper;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Analytics.DTOs;

namespace LifeManager.Infrastructure.Services;

public class AnalyticsReadService : IAnalyticsReadService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AnalyticsReadService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    private static DateTime GetPeriodStart(string period) => period.ToLowerInvariant() switch
    {
        "day"   => DateTime.UtcNow.Date,
        "month" => DateTime.UtcNow.Date.AddDays(-30),
        "year"  => DateTime.UtcNow.Date.AddDays(-365),
        _       => DateTime.UtcNow.Date.AddDays(-7) // "week" default
    };

    public async Task<ProductivityDto> GetProductivityAsync(Guid userId, string period, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        var startDate = GetPeriodStart(period);
        var now = DateTime.UtcNow;
        var param = new { UserId = userId, StartDate = startDate, Now = now };

        const string tasksByDateSql = @"
            SELECT pt.""CompletedAt""::date AS ""Date"", COUNT(*)::int AS ""Count""
            FROM ""ProjectTasks"" pt
            INNER JOIN ""Projects"" p ON p.""Id"" = pt.""ProjectId""
            WHERE p.""UserId"" = @UserId
              AND pt.""CompletedAt"" >= @StartDate
              AND pt.""CompletedAt"" IS NOT NULL
            GROUP BY pt.""CompletedAt""::date
            ORDER BY pt.""CompletedAt""::date;";

        const string timeByProjectSql = @"
            SELECT p.""Id"" AS ""ProjectId"", p.""Name"" AS ""ProjectName"",
                   COALESCE(SUM(te.""DurationMinutes""), 0)::int AS ""TotalMinutes""
            FROM ""TimeEntries"" te
            INNER JOIN ""ProjectTasks"" pt ON pt.""Id"" = te.""TaskId""
            INNER JOIN ""Projects"" p ON p.""Id"" = pt.""ProjectId""
            WHERE te.""UserId"" = @UserId
              AND te.""StartedAt"" >= @StartDate
              AND te.""DurationMinutes"" IS NOT NULL
            GROUP BY p.""Id"", p.""Name""
            ORDER BY ""TotalMinutes"" DESC;";

        const string overdueSql = @"
            SELECT
              COUNT(*) FILTER (WHERE pt.""CompletedAt"" > pt.""DueDate"")::int AS ""OverdueCompletedCount"",
              COUNT(*) FILTER (WHERE pt.""CompletedAt"" <= pt.""DueDate"")::int AS ""OnTimeCompletedCount"",
              COUNT(*)::int AS ""TotalTasksCompleted""
            FROM ""ProjectTasks"" pt
            INNER JOIN ""Projects"" p ON p.""Id"" = pt.""ProjectId""
            WHERE p.""UserId"" = @UserId
              AND pt.""CompletedAt"" >= @StartDate
              AND pt.""CompletedAt"" IS NOT NULL
              AND pt.""DueDate"" IS NOT NULL;";

        var tasksByDate = (await conn.QueryAsync<DailyTaskCountDto>(
            new CommandDefinition(tasksByDateSql, param, cancellationToken: ct))).ToList();

        var timeByProject = (await conn.QueryAsync<ProjectTimeDto>(
            new CommandDefinition(timeByProjectSql, param, cancellationToken: ct))).ToList();

        var overdueRow = await conn.QueryFirstOrDefaultAsync<(int OverdueCompletedCount, int OnTimeCompletedCount, int TotalTasksCompleted)>(
            new CommandDefinition(overdueSql, param, cancellationToken: ct));

        return new ProductivityDto
        {
            TasksCompletedByDate = tasksByDate,
            TimeTrackedByProject = timeByProject,
            TotalTasksCompleted = overdueRow.TotalTasksCompleted,
            OverdueCompletedCount = overdueRow.OverdueCompletedCount,
            OnTimeCompletedCount = overdueRow.OnTimeCompletedCount
        };
    }

    public async Task<AcademicAnalyticsDto?> GetAcademicAsync(Guid userId, Guid projectId, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        var now = DateTime.UtcNow;
        var param = new { UserId = userId, ProjectId = projectId, Now = now };

        const string detailSql = @"
            SELECT p.""Id"" AS ""ProjectId"", p.""Name"" AS ""ProjectName"",
                   ccd.""CurrentGrade"", ccd.""TargetGrade""
            FROM ""CollegeCourseDetails"" ccd
            INNER JOIN ""Projects"" p ON p.""Id"" = ccd.""ProjectId""
            WHERE ccd.""ProjectId"" = @ProjectId AND p.""UserId"" = @UserId;";

        const string taskStatsSql = @"
            SELECT
              COUNT(*)::int AS ""TotalTasks"",
              COUNT(*) FILTER (WHERE pt.""Status"" = 'Done')::int AS ""CompletedTasks"",
              COUNT(*) FILTER (WHERE pt.""DueDate"" < @Now AND pt.""Status"" != 'Done')::int AS ""OverdueTasks"",
              COUNT(*) FILTER (WHERE pt.""Status"" != 'Done')::int AS ""PendingTasks""
            FROM ""ProjectTasks"" pt
            WHERE pt.""ProjectId"" = @ProjectId;";

        const string prioritySql = @"
            SELECT pt.""Priority"", COUNT(*)::int AS ""Count""
            FROM ""ProjectTasks"" pt
            WHERE pt.""ProjectId"" = @ProjectId
            GROUP BY pt.""Priority"";";

        const string recentSql = @"
            SELECT pt.""Id"", pt.""Title"", pt.""CompletedAt""
            FROM ""ProjectTasks"" pt
            WHERE pt.""ProjectId"" = @ProjectId
              AND pt.""CompletedAt"" IS NOT NULL
            ORDER BY pt.""CompletedAt"" DESC
            LIMIT 10;";

        var detail = await conn.QueryFirstOrDefaultAsync<(Guid ProjectId, string ProjectName, decimal? CurrentGrade, decimal? TargetGrade)>(
            new CommandDefinition(detailSql, param, cancellationToken: ct));

        if (detail.ProjectId == Guid.Empty)
            return null;

        var stats = await conn.QueryFirstAsync<(int TotalTasks, int CompletedTasks, int OverdueTasks, int PendingTasks)>(
            new CommandDefinition(taskStatsSql, param, cancellationToken: ct));

        var byPriority = (await conn.QueryAsync<PriorityCountDto>(
            new CommandDefinition(prioritySql, param, cancellationToken: ct))).ToList();

        var recents = (await conn.QueryAsync<RecentCompletionDto>(
            new CommandDefinition(recentSql, param, cancellationToken: ct))).ToList();

        return new AcademicAnalyticsDto
        {
            ProjectId = detail.ProjectId,
            ProjectName = detail.ProjectName,
            CurrentGrade = detail.CurrentGrade,
            TargetGrade = detail.TargetGrade,
            TotalTasks = stats.TotalTasks,
            CompletedTasks = stats.CompletedTasks,
            OverdueTasks = stats.OverdueTasks,
            PendingTasks = stats.PendingTasks,
            TasksByPriority = byPriority,
            RecentCompletions = recents
        };
    }

    public async Task<ExerciseAnalyticsDto> GetExerciseAsync(Guid userId, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        var weekStart = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
        var twelveWeeksAgo = DateTime.UtcNow.Date.AddDays(-84);
        var param = new { UserId = userId, WeekStart = weekStart, TwelveWeeksAgo = twelveWeeksAgo };

        const string thisWeekSql = @"
            SELECT COUNT(*)::int
            FROM ""WorkoutSessions"" ws
            WHERE ws.""UserId"" = @UserId
              AND ws.""StartedAt"" >= @WeekStart;";

        const string byWeekSql = @"
            SELECT DATE_TRUNC('week', ws.""StartedAt"")::date AS ""WeekStart"",
                   COUNT(*)::int AS ""Count""
            FROM ""WorkoutSessions"" ws
            WHERE ws.""UserId"" = @UserId
              AND ws.""StartedAt"" >= @TwelveWeeksAgo
            GROUP BY DATE_TRUNC('week', ws.""StartedAt"")
            ORDER BY ""WeekStart"";";

        var thisWeekCount = await conn.QueryFirstAsync<int>(
            new CommandDefinition(thisWeekSql, param, cancellationToken: ct));

        var byWeek = (await conn.QueryAsync<WorkoutWeekDto>(
            new CommandDefinition(byWeekSql, param, cancellationToken: ct))).ToList();

        return new ExerciseAnalyticsDto
        {
            WorkoutSessionsThisWeek = thisWeekCount,
            WorkoutSessionsByWeek = byWeek
        };
    }

    public async Task<ProjectHealthDto> GetProjectHealthAsync(Guid userId, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        var now = DateTime.UtcNow;
        var param = new { UserId = userId, Now = now };

        const string projectsSql = @"
            SELECT
              p.""Id"" AS ""ProjectId"",
              p.""Name"",
              p.""Type"",
              p.""Status"",
              COUNT(pt.""Id"")::int AS ""TaskCount"",
              COUNT(pt.""Id"") FILTER (WHERE pt.""Status"" = 'Done')::int AS ""CompletedCount"",
              COUNT(pt.""Id"") FILTER (WHERE pt.""DueDate"" < @Now AND pt.""Status"" != 'Done')::int AS ""OverdueCount""
            FROM ""Projects"" p
            LEFT JOIN ""ProjectTasks"" pt ON pt.""ProjectId"" = p.""Id""
            WHERE p.""UserId"" = @UserId AND p.""Status"" = 'Active'
            GROUP BY p.""Id"", p.""Name"", p.""Type"", p.""Status""
            ORDER BY p.""Name"";";

        var projects = (await conn.QueryAsync<ProjectHealthItemDto>(
            new CommandDefinition(projectsSql, param, cancellationToken: ct))).ToList();

        return new ProjectHealthDto
        {
            TotalActiveProjects = projects.Count,
            TotalOverdueTasksAcrossAll = projects.Sum(p => p.OverdueCount),
            Projects = projects
        };
    }

    public async Task<TimeTrackingAnalyticsDto> GetTimeTrackingAsync(Guid userId, string period, string groupBy, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        var startDate = GetPeriodStart(period);
        var param = new { UserId = userId, StartDate = startDate };

        string sql = groupBy.ToLowerInvariant() == "day"
            ? @"
                SELECT te.""StartedAt""::date::text AS ""Dimension"",
                       COALESCE(SUM(te.""DurationMinutes""), 0)::int AS ""TotalMinutes""
                FROM ""TimeEntries"" te
                INNER JOIN ""ProjectTasks"" pt ON pt.""Id"" = te.""TaskId""
                INNER JOIN ""Projects"" p ON p.""Id"" = pt.""ProjectId""
                WHERE te.""UserId"" = @UserId
                  AND te.""StartedAt"" >= @StartDate
                  AND te.""DurationMinutes"" IS NOT NULL
                GROUP BY te.""StartedAt""::date
                ORDER BY te.""StartedAt""::date;"
            : @"
                SELECT p.""Name"" AS ""Dimension"",
                       COALESCE(SUM(te.""DurationMinutes""), 0)::int AS ""TotalMinutes""
                FROM ""TimeEntries"" te
                INNER JOIN ""ProjectTasks"" pt ON pt.""Id"" = te.""TaskId""
                INNER JOIN ""Projects"" p ON p.""Id"" = pt.""ProjectId""
                WHERE te.""UserId"" = @UserId
                  AND te.""StartedAt"" >= @StartDate
                  AND te.""DurationMinutes"" IS NOT NULL
                GROUP BY p.""Name""
                ORDER BY ""TotalMinutes"" DESC;";

        var entries = (await conn.QueryAsync<TimeTrackingEntryDto>(
            new CommandDefinition(sql, param, cancellationToken: ct))).ToList();

        return new TimeTrackingAnalyticsDto
        {
            TotalMinutes = entries.Sum(e => e.TotalMinutes),
            Entries = entries
        };
    }
}
