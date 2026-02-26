using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Labels.Commands;

public record DeleteLabelCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteLabelCommandHandler : IRequestHandler<DeleteLabelCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteLabelCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteLabelCommand request, CancellationToken ct)
    {
        var label = await _uow.Labels.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Label", request.Id);

        // Delete cascades to ProjectLabels and TaskLabels via DB foreign keys
        _uow.Labels.Delete(label);
        await _uow.SaveChangesAsync(ct);
    }
}
