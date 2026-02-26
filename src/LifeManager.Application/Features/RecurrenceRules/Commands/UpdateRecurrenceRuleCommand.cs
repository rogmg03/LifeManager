using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.RecurrenceRules.DTOs;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.RecurrenceRules.Commands;

public record UpdateRecurrenceRuleCommand(
    Guid TaskId,
    RecurrencePattern Pattern,
    DateOnly NextDueDate,
    int? IntervalDays,
    string? DaysOfWeek,
    bool IsActive) : IRequest<RecurrenceRuleDto>, IBaseCommand;

public class UpdateRecurrenceRuleCommandHandler : IRequestHandler<UpdateRecurrenceRuleCommand, RecurrenceRuleDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateRecurrenceRuleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<RecurrenceRuleDto> Handle(UpdateRecurrenceRuleCommand request, CancellationToken ct)
    {
        var rule = await _uow.RecurrenceRules.GetByTaskIdAsync(request.TaskId, ct)
            ?? throw new NotFoundException("RecurrenceRule", request.TaskId);

        rule.Pattern = request.Pattern;
        rule.NextDueDate = request.NextDueDate;
        rule.IntervalDays = request.IntervalDays;
        rule.DaysOfWeek = request.DaysOfWeek;
        rule.IsActive = request.IsActive;

        _uow.RecurrenceRules.Update(rule);
        await _uow.SaveChangesAsync(ct);
        return RecurrenceRuleDto.FromEntity(rule);
    }
}
