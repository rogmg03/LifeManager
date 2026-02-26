using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Labels.Commands;

public record AssignLabelToProjectCommand(Guid ProjectId, Guid LabelId) : IRequest, IBaseCommand;

public class AssignLabelToProjectCommandHandler : IRequestHandler<AssignLabelToProjectCommand>
{
    private readonly IUnitOfWork _uow;
    public AssignLabelToProjectCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(AssignLabelToProjectCommand request, CancellationToken ct)
    {
        var project = await _uow.Projects.GetByIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("Project", request.ProjectId);

        var label = await _uow.Labels.GetByIdAsync(request.LabelId, ct)
            ?? throw new NotFoundException("Label", request.LabelId);

        await _uow.Labels.AddProjectLabelAsync(project.Id, label.Id, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
