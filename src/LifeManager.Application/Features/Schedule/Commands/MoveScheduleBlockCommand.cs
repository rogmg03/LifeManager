using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Schedule.DTOs;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Schedule.Commands;

public record MoveScheduleBlockCommand(
    Guid Id,
    DateTime StartTime,
    DateTime EndTime) : IRequest<ScheduleBlockDto>, IBaseCommand;

public class MoveScheduleBlockCommandHandler : IRequestHandler<MoveScheduleBlockCommand, ScheduleBlockDto>
{
    private readonly IUnitOfWork _uow;
    public MoveScheduleBlockCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ScheduleBlockDto> Handle(MoveScheduleBlockCommand request, CancellationToken ct)
    {
        var block = await _uow.ScheduleBlocks.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("ScheduleBlock", request.Id);

        block.StartTime = request.StartTime;
        block.EndTime = request.EndTime;
        _uow.ScheduleBlocks.Update(block);

        // Auto-resolve overlap: shift overlapping Flexible blocks to start after this block ends
        var dayStart = request.StartTime.Date;
        var dayEnd = dayStart.AddDays(1);
        var dayBlocks = await _uow.ScheduleBlocks.GetByDateRangeAsync(block.UserId, dayStart, dayEnd, ct);

        foreach (var other in dayBlocks)
        {
            if (other.Id == block.Id || other.BlockType != BlockType.Flexible)
                continue;

            bool overlaps = other.StartTime < request.EndTime && other.EndTime > request.StartTime;
            if (!overlaps) continue;

            var duration = other.EndTime - other.StartTime;
            other.StartTime = request.EndTime;
            other.EndTime = request.EndTime + duration;
            _uow.ScheduleBlocks.Update(other);
        }

        await _uow.SaveChangesAsync(ct);
        return ScheduleBlockDto.FromEntity(block);
    }
}
