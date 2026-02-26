using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.TimeTracking.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.TimeTracking.Queries;

public record GetTaskTimeEntriesQuery(Guid TaskId) : IRequest<List<TimeEntryDto>>;

public class GetTaskTimeEntriesQueryHandler : IRequestHandler<GetTaskTimeEntriesQuery, List<TimeEntryDto>>
{
    private readonly IUnitOfWork _uow;

    public GetTaskTimeEntriesQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<List<TimeEntryDto>> Handle(GetTaskTimeEntriesQuery request, CancellationToken ct)
    {
        var task = await _uow.Tasks.GetByIdAsync(request.TaskId, ct)
            ?? throw new NotFoundException("Task", request.TaskId);

        var project = await _uow.Projects.GetByIdAsync(task.ProjectId, ct);
        var entries = await _uow.TimeEntries.GetByTaskIdAsync(request.TaskId, ct);

        return entries
            .Select(e => TimeEntryDto.FromEntityWithTask(
                e, task.Title, task.ProjectId, project?.Name ?? string.Empty))
            .ToList();
    }
}
