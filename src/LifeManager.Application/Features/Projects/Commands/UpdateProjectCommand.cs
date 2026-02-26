using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Projects.DTOs;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Projects.Commands;

public record UpdateProjectCommand(
    Guid Id,
    string Name,
    string? Description,
    ProjectType Type,
    ProjectStatus Status,
    Guid? ClientId,
    string? Color,
    DateOnly? StartDate,
    DateOnly? EndDate) : IRequest<ProjectDto>, IBaseCommand;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateProjectCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Project", request.Id);

        project.Name = request.Name;
        project.Description = request.Description;
        project.Type = request.Type;
        project.Status = request.Status;
        project.ClientId = request.ClientId;
        project.Color = request.Color;
        project.StartDate = request.StartDate;
        project.EndDate = request.EndDate;

        _uow.Projects.Update(project);
        await _uow.SaveChangesAsync(ct);
        return ProjectDto.FromEntity(project);
    }
}
