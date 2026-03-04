using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.AssignmentDetails.Commands;

public record DeleteAssignmentDetailCommand(Guid TaskId) : IRequest, IBaseCommand;

public class DeleteAssignmentDetailCommandHandler : IRequestHandler<DeleteAssignmentDetailCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteAssignmentDetailCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteAssignmentDetailCommand request, CancellationToken ct)
    {
        var detail = await _uow.AssignmentDetails.GetByTaskIdAsync(request.TaskId, ct)
            ?? throw new NotFoundException("AssignmentDetail", request.TaskId);
        _uow.AssignmentDetails.Delete(detail);
        await _uow.SaveChangesAsync(ct);
    }
}
