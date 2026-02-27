using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Reminders.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Reminders.Queries;

public record GetReminderByIdQuery(Guid Id) : IRequest<ReminderDto>;

public class GetReminderByIdQueryHandler : IRequestHandler<GetReminderByIdQuery, ReminderDto>
{
    private readonly IUnitOfWork _uow;
    public GetReminderByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ReminderDto> Handle(GetReminderByIdQuery request, CancellationToken ct)
    {
        var reminder = await _uow.Reminders.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("Reminder", request.Id);
        return ReminderDto.FromEntity(reminder);
    }
}
