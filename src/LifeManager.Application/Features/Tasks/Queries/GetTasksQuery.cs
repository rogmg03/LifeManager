using LifeManager.Application.Features.Tasks.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Tasks.Queries;

public record GetTasksQuery(Guid ProjectId) : IRequest<List<TaskDto>>;

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, List<TaskDto>>
{
    private readonly IUnitOfWork _uow;
    public GetTasksQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<List<TaskDto>> Handle(GetTasksQuery request, CancellationToken ct)
    {
        var tasks = await _uow.Tasks.GetAllByProjectIdAsync(request.ProjectId, ct);
        return tasks.Select(TaskDto.FromEntity).ToList();
    }
}
