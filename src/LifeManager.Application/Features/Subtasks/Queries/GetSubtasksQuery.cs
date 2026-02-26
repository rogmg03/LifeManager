using LifeManager.Application.Features.Subtasks.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Subtasks.Queries;

public record GetSubtasksQuery(Guid TaskId) : IRequest<List<SubtaskDto>>;

public class GetSubtasksQueryHandler : IRequestHandler<GetSubtasksQuery, List<SubtaskDto>>
{
    private readonly IUnitOfWork _uow;
    public GetSubtasksQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<List<SubtaskDto>> Handle(GetSubtasksQuery request, CancellationToken ct)
    {
        var subtasks = await _uow.Subtasks.GetAllByTaskIdAsync(request.TaskId, ct);
        return subtasks.Select(SubtaskDto.FromEntity).ToList();
    }
}
