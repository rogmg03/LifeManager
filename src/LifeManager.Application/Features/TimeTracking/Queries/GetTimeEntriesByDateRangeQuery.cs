using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.TimeTracking.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.TimeTracking.Queries;

public record GetTimeEntriesByDateRangeQuery(DateTime From, DateTime To) : IRequest<List<TimeEntriesByProjectDto>>;

public class GetTimeEntriesByDateRangeQueryHandler
    : IRequestHandler<GetTimeEntriesByDateRangeQuery, List<TimeEntriesByProjectDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetTimeEntriesByDateRangeQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<List<TimeEntriesByProjectDto>> Handle(
        GetTimeEntriesByDateRangeQuery request, CancellationToken ct)
    {
        var entries = await _uow.TimeEntries.GetByDateRangeAsync(
            _currentUser.UserId, request.From, request.To, ct);

        // Group by project (navigations Task + Task.Project are loaded by repository)
        var grouped = entries
            .GroupBy(e => e.Task.ProjectId)
            .Select(g =>
            {
                var projectName = g.First().Task.Project?.Name ?? string.Empty;
                var dtos = g.Select(TimeEntryDto.FromEntity).ToList();
                var totalMinutes = dtos.Sum(d => d.DurationMinutes ?? 0);
                return new TimeEntriesByProjectDto(g.Key, projectName, totalMinutes, dtos);
            })
            .OrderByDescending(g => g.TotalMinutes)
            .ToList();

        return grouped;
    }
}
