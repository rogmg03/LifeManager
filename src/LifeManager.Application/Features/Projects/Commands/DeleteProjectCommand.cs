using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Projects.Commands;

public record DeleteProjectCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteProjectCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteProjectCommand request, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Project", request.Id);

        // Soft delete — archive
        project.Status = ProjectStatus.Archived;
        _uow.Projects.Update(project);
        await _uow.SaveChangesAsync(ct);
    }
}
