using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Activity.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Activity.Queries;

public record GetProjectActivityQuery(Guid ProjectId) : IRequest<List<ActivityEntryDto>>;

public class GetProjectActivityQueryHandler
    : IRequestHandler<GetProjectActivityQuery, List<ActivityEntryDto>>
{
    private readonly IUnitOfWork _uow;

    public GetProjectActivityQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<List<ActivityEntryDto>> Handle(
        GetProjectActivityQuery request, CancellationToken ct)
    {
        _ = await _uow.Projects.GetByIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("Project", request.ProjectId);

        var entries = await _uow.ActivityEntries.GetByProjectIdAsync(request.ProjectId, ct);
        return entries.Select(ActivityEntryDto.FromEntity).ToList();
    }
}
