using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Schedule.DTOs;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Events;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Schedule.Commands;

public record UpdateScheduleBlockStatusCommand(Guid Id, BlockStatus Status) : IRequest<ScheduleBlockDto>, IBaseCommand;

public class UpdateScheduleBlockStatusCommandHandler : IRequestHandler<UpdateScheduleBlockStatusCommand, ScheduleBlockDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateScheduleBlockStatusCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ScheduleBlockDto> Handle(UpdateScheduleBlockStatusCommand request, CancellationToken ct)
    {
        var block = await _uow.ScheduleBlocks.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("ScheduleBlock", request.Id);

        block.Status = request.Status;

        if (request.Status == BlockStatus.Skipped)
            block.AddDomainEvent(new ScheduleBlockSkippedEvent(
                block.Id, block.UserId, block.Title, block.StartTime, block.EndTime));

        _uow.ScheduleBlocks.Update(block);
        await _uow.SaveChangesAsync(ct);

        return ScheduleBlockDto.FromEntity(block);
    }
}
