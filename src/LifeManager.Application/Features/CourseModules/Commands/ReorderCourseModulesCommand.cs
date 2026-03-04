using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.CourseModules.Commands;

public record ReorderCourseModulesCommand(List<(Guid Id, int SortOrder)> Items) : IRequest, IBaseCommand;

public class ReorderCourseModulesCommandHandler : IRequestHandler<ReorderCourseModulesCommand>
{
    private readonly IUnitOfWork _uow;
    public ReorderCourseModulesCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(ReorderCourseModulesCommand request, CancellationToken ct)
    {
        foreach (var (id, sortOrder) in request.Items)
        {
            var module = await _uow.CourseModules.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("CourseModule", id);
            module.SortOrder = sortOrder;
            _uow.CourseModules.Update(module);
        }
        await _uow.SaveChangesAsync(ct);
    }
}
