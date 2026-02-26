using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.RecurrenceRules.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.RecurrenceRules.Queries;

public record GetRecurrenceRuleQuery(Guid TaskId) : IRequest<RecurrenceRuleDto>;

public class GetRecurrenceRuleQueryHandler : IRequestHandler<GetRecurrenceRuleQuery, RecurrenceRuleDto>
{
    private readonly IUnitOfWork _uow;
    public GetRecurrenceRuleQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<RecurrenceRuleDto> Handle(GetRecurrenceRuleQuery request, CancellationToken ct)
    {
        var rule = await _uow.RecurrenceRules.GetByTaskIdAsync(request.TaskId, ct)
            ?? throw new NotFoundException("RecurrenceRule", request.TaskId);
        return RecurrenceRuleDto.FromEntity(rule);
    }
}
