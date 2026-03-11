using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Projects.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Projects.Queries;

public record GetProjectsQuery : IRequest<List<ProjectDto>>;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, List<ProjectDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetProjectsQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<List<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken ct)
    {
        var projects = await _uow.Projects.GetAllByUserIdAsync(_currentUser.UserId, ct);
        if (!projects.Any()) return [];

        var counts = await _uow.Projects.GetTaskCountsAsync(projects.Select(p => p.Id), ct);

        return projects.Select(p =>
        {
            var dto = ProjectDto.FromEntity(p);
            if (counts.TryGetValue(p.Id, out var c))
                dto = dto with
                {
                    TotalTasks = c.TotalTasks,
                    CompletedTasks = c.CompletedTasks,
                    OverdueTasks = c.OverdueTasks,
                    TotalTimeTrackedMinutes = c.TotalTimeTrackedMinutes,
                };
            return dto;
        }).ToList();
    }
}
