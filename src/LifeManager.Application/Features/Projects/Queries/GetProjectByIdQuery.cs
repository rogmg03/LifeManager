using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Projects.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Projects.Queries;

public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectDto>;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
{
    private readonly IUnitOfWork _uow;
    public GetProjectByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Project", request.Id);
        return ProjectDto.FromEntity(project);
    }
}
