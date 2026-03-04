using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.CourseModules.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.CourseModules.Commands;

public record CreateCourseModuleCommand(
    Guid ProjectId,
    string Name,
    string? Description,
    int SortOrder,
    string? Notes) : IRequest<CourseModuleDto>, IBaseCommand;

public class CreateCourseModuleCommandHandler : IRequestHandler<CreateCourseModuleCommand, CourseModuleDto>
{
    private readonly IUnitOfWork _uow;
    public CreateCourseModuleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CourseModuleDto> Handle(CreateCourseModuleCommand request, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("Project", request.ProjectId);

        if (project.Type != ProjectType.OnlineCourse)
            throw new BadRequestException("Project is not an Online Course type.");

        var module = new CourseModule
        {
            ProjectId = request.ProjectId,
            Name = request.Name,
            Description = request.Description,
            SortOrder = request.SortOrder,
            IsCompleted = false,
            Notes = request.Notes
        };

        await _uow.CourseModules.AddAsync(module, ct);
        await _uow.SaveChangesAsync(ct);
        return CourseModuleDto.FromEntity(module);
    }
}
