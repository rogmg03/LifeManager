using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.RecurrenceRules.Commands;

public record DeleteRecurrenceRuleCommand(Guid TaskId) : IRequest, IBaseCommand;

public class DeleteRecurrenceRuleCommandHandler : IRequestHandler<DeleteRecurrenceRuleCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteRecurrenceRuleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteRecurrenceRuleCommand request, CancellationToken ct)
    {
        var rule = await _uow.RecurrenceRules.GetByTaskIdAsync(request.TaskId, ct)
            ?? throw new NotFoundException("RecurrenceRule", request.TaskId);

        _uow.RecurrenceRules.Delete(rule);
        await _uow.SaveChangesAsync(ct);
    }
}
