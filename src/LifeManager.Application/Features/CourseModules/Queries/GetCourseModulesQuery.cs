using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.CourseModules.DTOs;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.CourseModules.Queries;

public record GetCourseModulesQuery(Guid ProjectId) : IRequest<List<CourseModuleDto>>;

public class GetCourseModulesQueryHandler : IRequestHandler<GetCourseModulesQuery, List<CourseModuleDto>>
{
    private readonly IUnitOfWork _uow;
    public GetCourseModulesQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<List<CourseModuleDto>> Handle(GetCourseModulesQuery request, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("Project", request.ProjectId);

        if (project.Type != ProjectType.OnlineCourse)
            throw new BadRequestException("Project is not an Online Course type.");

        var modules = await _uow.CourseModules.GetByProjectIdAsync(request.ProjectId, ct);
        return modules.Select(CourseModuleDto.FromEntity).ToList();
    }
}
