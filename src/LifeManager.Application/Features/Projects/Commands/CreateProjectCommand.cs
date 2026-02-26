using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Projects.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Projects.Commands;

public record CreateProjectCommand(
    string Name,
    string? Description,
    ProjectType Type,
    Guid? ClientId,
    string? Color,
    DateOnly? StartDate,
    DateOnly? EndDate) : IRequest<ProjectDto>, IBaseCommand;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CreateProjectCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken ct)
    {
        var project = new Project
        {
            UserId = _currentUser.UserId,
            ClientId = request.ClientId,
            Name = request.Name,
            Description = request.Description,
            Type = request.Type,
            Color = request.Color,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };
        await _uow.Projects.AddAsync(project, ct);
        await _uow.SaveChangesAsync(ct);
        return ProjectDto.FromEntity(project);
    }
}
