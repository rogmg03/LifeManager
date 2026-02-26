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
        return projects.Select(ProjectDto.FromEntity).ToList();
    }
}
