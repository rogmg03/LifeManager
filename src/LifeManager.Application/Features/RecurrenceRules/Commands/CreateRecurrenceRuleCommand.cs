using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.RecurrenceRules.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.RecurrenceRules.Commands;

public record CreateRecurrenceRuleCommand(
    Guid TaskId,
    RecurrencePattern Pattern,
    DateOnly NextDueDate,
    int? IntervalDays,
    string? DaysOfWeek) : IRequest<RecurrenceRuleDto>, IBaseCommand;

public class CreateRecurrenceRuleCommandHandler : IRequestHandler<CreateRecurrenceRuleCommand, RecurrenceRuleDto>
{
    private readonly IUnitOfWork _uow;
    public CreateRecurrenceRuleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<RecurrenceRuleDto> Handle(CreateRecurrenceRuleCommand request, CancellationToken ct)
    {
        var existing = await _uow.RecurrenceRules.GetByTaskIdAsync(request.TaskId, ct);
        if (existing is not null)
            throw new InvalidOperationException($"Task {request.TaskId} already has a recurrence rule.");

        var rule = new RecurrenceRule
        {
            TaskId = request.TaskId,
            Pattern = request.Pattern,
            NextDueDate = request.NextDueDate,
            IntervalDays = request.IntervalDays,
            DaysOfWeek = request.DaysOfWeek
        };
        await _uow.RecurrenceRules.AddAsync(rule, ct);
        await _uow.SaveChangesAsync(ct);
        return RecurrenceRuleDto.FromEntity(rule);
    }
}
