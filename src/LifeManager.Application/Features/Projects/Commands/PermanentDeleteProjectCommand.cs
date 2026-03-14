using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Projects.Commands;

public record PermanentDeleteProjectCommand(Guid Id) : IRequest, IBaseCommand;

public class PermanentDeleteProjectCommandHandler : IRequestHandler<PermanentDeleteProjectCommand>
{
    private readonly IUnitOfWork _uow;
    public PermanentDeleteProjectCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(PermanentDeleteProjectCommand request, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Project", request.Id);

        _uow.Projects.Delete(project);
        await _uow.SaveChangesAsync(ct);
    }
}
