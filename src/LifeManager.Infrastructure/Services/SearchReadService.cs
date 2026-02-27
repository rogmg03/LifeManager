using Dapper;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Search.DTOs;

namespace LifeManager.Infrastructure.Services;

public class SearchReadService : ISearchReadService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SearchReadService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<SearchResultsDto> SearchAsync(Guid userId, string query, string? type, CancellationToken ct = default)
    {
        using var conn = _connectionFactory.CreateConnection();
        var param = new { UserId = userId, Query = query };

        var projects = (type is null || type == "project")
            ? (await conn.QueryAsync<SearchResultItemDto>(new CommandDefinition(ProjectsSql, param, cancellationToken: ct))).ToList()
            : (IList<SearchResultItemDto>)[];

        var tasks = (type is null || type == "task")
            ? (await conn.QueryAsync<SearchResultItemDto>(new CommandDefinition(TasksSql, param, cancellationToken: ct))).ToList()
            : (IList<SearchResultItemDto>)[];

        var documents = (type is null || type == "document")
            ? (await conn.QueryAsync<SearchResultItemDto>(new CommandDefinition(DocumentsSql, param, cancellationToken: ct))).ToList()
            : (IList<SearchResultItemDto>)[];

        var activities = (type is null || type == "activity")
            ? (await conn.QueryAsync<SearchResultItemDto>(new CommandDefinition(ActivitiesSql, param, cancellationToken: ct))).ToList()
            : (IList<SearchResultItemDto>)[];

        var clients = (type is null || type == "client")
            ? (await conn.QueryAsync<SearchResultItemDto>(new CommandDefinition(ClientsSql, param, cancellationToken: ct))).ToList()
            : (IList<SearchResultItemDto>)[];

        return new SearchResultsDto
        {
            Query = query,
            TotalCount = projects.Count + tasks.Count + documents.Count + activities.Count + clients.Count,
            Projects = projects,
            Tasks = tasks,
            Documents = documents,
            Activities = activities,
            Clients = clients
        };
    }

    private const string ProjectsSql = @"
        SELECT p.""Id"", 'project' AS ""Type"", p.""Name"" AS ""Title"",
               LEFT(p.""Description"", 200) AS ""Snippet"",
               NULL::uuid AS ""ProjectId"", NULL::text AS ""ProjectName""
        FROM ""Projects"" p
        WHERE p.""UserId"" = @UserId
          AND (p.""Name"" ILIKE '%' || @Query || '%'
               OR p.""Description"" ILIKE '%' || @Query || '%')
        ORDER BY p.""Name""
        LIMIT 10;";

    private const string TasksSql = @"
        SELECT pt.""Id"", 'task' AS ""Type"", pt.""Title"" AS ""Title"",
               LEFT(pt.""Description"", 200) AS ""Snippet"",
               p.""Id"" AS ""ProjectId"", p.""Name"" AS ""ProjectName""
        FROM ""ProjectTasks"" pt
        INNER JOIN ""Projects"" p ON p.""Id"" = pt.""ProjectId""
        WHERE p.""UserId"" = @UserId
          AND (pt.""Title"" ILIKE '%' || @Query || '%'
               OR pt.""Description"" ILIKE '%' || @Query || '%')
        ORDER BY pt.""Title""
        LIMIT 10;";

    private const string DocumentsSql = @"
        SELECT d.""Id"", 'document' AS ""Type"", d.""Title"" AS ""Title"",
               LEFT(d.""Content"", 200) AS ""Snippet"",
               p.""Id"" AS ""ProjectId"", p.""Name"" AS ""ProjectName""
        FROM ""Documents"" d
        INNER JOIN ""Projects"" p ON p.""Id"" = d.""ProjectId""
        WHERE p.""UserId"" = @UserId
          AND (d.""Title"" ILIKE '%' || @Query || '%'
               OR d.""Content"" ILIKE '%' || @Query || '%')
        ORDER BY d.""Title""
        LIMIT 10;";

    private const string ActivitiesSql = @"
        SELECT ae.""Id"", 'activity' AS ""Type"", ae.""Description"" AS ""Title"",
               NULL::text AS ""Snippet"",
               p.""Id"" AS ""ProjectId"", p.""Name"" AS ""ProjectName""
        FROM ""ActivityEntries"" ae
        LEFT JOIN ""Projects"" p ON p.""Id"" = ae.""ProjectId""
        WHERE ae.""UserId"" = @UserId
          AND ae.""Description"" ILIKE '%' || @Query || '%'
        ORDER BY ae.""CreatedAt"" DESC
        LIMIT 10;";

    private const string ClientsSql = @"
        SELECT c.""Id"", 'client' AS ""Type"", c.""Name"" AS ""Title"",
               c.""Notes"" AS ""Snippet"",
               NULL::uuid AS ""ProjectId"", NULL::text AS ""ProjectName""
        FROM ""Clients"" c
        WHERE c.""UserId"" = @UserId
          AND (c.""Name"" ILIKE '%' || @Query || '%'
               OR c.""ContactPerson"" ILIKE '%' || @Query || '%'
               OR c.""Notes"" ILIKE '%' || @Query || '%')
        ORDER BY c.""Name""
        LIMIT 10;";
}
