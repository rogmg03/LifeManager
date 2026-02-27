using System.Text;
using Dapper;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Export.DTOs;

namespace LifeManager.Infrastructure.Services;

public class ExportService : IExportService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ExportService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<ExportDataDto> GetAllDataAsync(Guid userId, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new { UserId = userId };

        var projects = (await conn.QueryAsync<ExportProjectRow>(
            new CommandDefinition(ProjectsSql, p, cancellationToken: ct))).ToList();

        var tasks = (await conn.QueryAsync<ExportTaskRow>(
            new CommandDefinition(TasksSql, p, cancellationToken: ct))).ToList();

        var clients = (await conn.QueryAsync<ExportClientRow>(
            new CommandDefinition(ClientsSql, p, cancellationToken: ct))).ToList();

        var timeEntries = (await conn.QueryAsync<ExportTimeEntryRow>(
            new CommandDefinition(TimeEntriesSql, p, cancellationToken: ct))).ToList();

        var scheduleBlocks = (await conn.QueryAsync<ExportScheduleBlockRow>(
            new CommandDefinition(ScheduleBlocksSql, p, cancellationToken: ct))).ToList();

        var labels = (await conn.QueryAsync<ExportLabelRow>(
            new CommandDefinition(LabelsSql, p, cancellationToken: ct))).ToList();

        var documents = (await conn.QueryAsync<ExportDocumentRow>(
            new CommandDefinition(DocumentsSql, p, cancellationToken: ct))).ToList();

        return new ExportDataDto
        {
            Projects = projects,
            Tasks = tasks,
            Clients = clients,
            TimeEntries = timeEntries,
            ScheduleBlocks = scheduleBlocks,
            Labels = labels,
            Documents = documents
        };
    }

    public async Task<string> GetCsvAsync(Guid userId, string entity, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        var p = new { UserId = userId };

        return entity switch
        {
            "projects" => ToCsv(await conn.QueryAsync<ExportProjectRow>(
                new CommandDefinition(ProjectsSql, p, cancellationToken: ct))),

            "tasks" => ToCsv(await conn.QueryAsync<ExportTaskRow>(
                new CommandDefinition(TasksSql, p, cancellationToken: ct))),

            "clients" => ToCsv(await conn.QueryAsync<ExportClientRow>(
                new CommandDefinition(ClientsSql, p, cancellationToken: ct))),

            "time-entries" => ToCsv(await conn.QueryAsync<ExportTimeEntryRow>(
                new CommandDefinition(TimeEntriesSql, p, cancellationToken: ct))),

            "schedule-blocks" => ToCsv(await conn.QueryAsync<ExportScheduleBlockRow>(
                new CommandDefinition(ScheduleBlocksSql, p, cancellationToken: ct))),

            "labels" => ToCsv(await conn.QueryAsync<ExportLabelRow>(
                new CommandDefinition(LabelsSql, p, cancellationToken: ct))),

            "documents" => ToCsv(await conn.QueryAsync<ExportDocumentRow>(
                new CommandDefinition(DocumentsSql, p, cancellationToken: ct))),

            _ => throw new ArgumentException($"Unknown entity: {entity}")
        };
    }

    private static string ToCsv<T>(IEnumerable<T> rows)
    {
        var sb = new StringBuilder();
        var properties = typeof(T).GetProperties();

        sb.AppendLine(string.Join(",", properties.Select(prop => QuoteCsv(prop.Name))));

        foreach (var row in rows)
            sb.AppendLine(string.Join(",", properties.Select(prop => QuoteCsv(prop.GetValue(row)?.ToString()))));

        return sb.ToString();
    }

    private static string QuoteCsv(string? value)
    {
        if (value is null) return "";
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";
        return value;
    }

    private const string ProjectsSql = @"
        SELECT p.""Id"", p.""Name"", p.""Description"", p.""Type"", p.""Status"",
               p.""Color"",
               p.""StartDate""::timestamptz AS ""StartDate"",
               p.""EndDate""::timestamptz AS ""EndDate"",
               p.""CreatedAt""
        FROM ""Projects"" p
        WHERE p.""UserId"" = @UserId
        ORDER BY p.""Name"";";

    private const string TasksSql = @"
        SELECT pt.""Id"", pt.""ProjectId"", p.""Name"" AS ""ProjectName"",
               pt.""Title"", pt.""Description"", pt.""Status"", pt.""Priority"",
               pt.""DueDate"", pt.""EstimatedMinutes"", pt.""CompletedAt"", pt.""CreatedAt""
        FROM ""ProjectTasks"" pt
        INNER JOIN ""Projects"" p ON p.""Id"" = pt.""ProjectId""
        WHERE p.""UserId"" = @UserId
        ORDER BY p.""Name"", pt.""CreatedAt"";";

    private const string ClientsSql = @"
        SELECT c.""Id"", c.""Name"", c.""ContactPerson"", c.""Priority"", c.""Status"",
               c.""Notes"", c.""Color"", c.""CreatedAt""
        FROM ""Clients"" c
        WHERE c.""UserId"" = @UserId
        ORDER BY c.""Name"";";

    private const string TimeEntriesSql = @"
        SELECT te.""Id"", te.""TaskId"", pt.""Title"" AS ""TaskTitle"", p.""Name"" AS ""ProjectName"",
               te.""StartedAt"", te.""EndedAt"", te.""DurationMinutes"", te.""Notes"", te.""CreatedAt""
        FROM ""TimeEntries"" te
        INNER JOIN ""ProjectTasks"" pt ON pt.""Id"" = te.""TaskId""
        INNER JOIN ""Projects"" p ON p.""Id"" = pt.""ProjectId""
        WHERE te.""UserId"" = @UserId
        ORDER BY te.""StartedAt"" DESC;";

    private const string ScheduleBlocksSql = @"
        SELECT sb.""Id"", sb.""Title"", sb.""Description"", sb.""BlockType"", sb.""Status"",
               sb.""StartTime"", sb.""EndTime"", sb.""CreatedAt""
        FROM ""ScheduleBlocks"" sb
        WHERE sb.""UserId"" = @UserId
        ORDER BY sb.""StartTime"" DESC;";

    private const string LabelsSql = @"
        SELECT l.""Id"", l.""Name"", l.""Color"", l.""CreatedAt""
        FROM ""Labels"" l
        WHERE l.""UserId"" = @UserId
        ORDER BY l.""Name"";";

    private const string DocumentsSql = @"
        SELECT d.""Id"", d.""ProjectId"", p.""Name"" AS ""ProjectName"",
               d.""Title"", LEFT(d.""Content"", 200) AS ""ContentSnippet"",
               d.""CreatedAt"", d.""UpdatedAt""
        FROM ""Documents"" d
        INNER JOIN ""Projects"" p ON p.""Id"" = d.""ProjectId""
        WHERE p.""UserId"" = @UserId
        ORDER BY p.""Name"", d.""Title"";";
}
