using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.CourseModules.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.CourseModules.Commands;

public record UpdateCourseModuleCommand(
    Guid ModuleId,
    string Name,
    string? Description,
    bool IsCompleted,
    string? Notes) : IRequest<CourseModuleDto>, IBaseCommand;

public class UpdateCourseModuleCommandHandler : IRequestHandler<UpdateCourseModuleCommand, CourseModuleDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateCourseModuleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CourseModuleDto> Handle(UpdateCourseModuleCommand request, CancellationToken ct)
    {
        var module = await _uow.CourseModules.GetByIdAsync(request.ModuleId, ct)
            ?? throw new NotFoundException("CourseModule", request.ModuleId);

        module.Name = request.Name;
        module.Description = request.Description;
        module.IsCompleted = request.IsCompleted;
        module.Notes = request.Notes;

        _uow.CourseModules.Update(module);
        await _uow.SaveChangesAsync(ct);
        return CourseModuleDto.FromEntity(module);
    }
}
