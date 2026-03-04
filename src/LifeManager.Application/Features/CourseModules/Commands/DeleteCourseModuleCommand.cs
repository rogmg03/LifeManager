using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.CourseModules.Commands;

public record DeleteCourseModuleCommand(Guid ModuleId) : IRequest, IBaseCommand;

public class DeleteCourseModuleCommandHandler : IRequestHandler<DeleteCourseModuleCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteCourseModuleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteCourseModuleCommand request, CancellationToken ct)
    {
        var module = await _uow.CourseModules.GetByIdAsync(request.ModuleId, ct)
            ?? throw new NotFoundException("CourseModule", request.ModuleId);
        _uow.CourseModules.Delete(module);
        await _uow.SaveChangesAsync(ct);
    }
}
