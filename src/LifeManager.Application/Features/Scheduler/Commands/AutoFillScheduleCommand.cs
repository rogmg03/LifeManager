using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Schedule.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Scheduler.Commands;

public record AutoFillScheduleCommand(DateOnly Date) : IRequest<List<ScheduleBlockDto>>, IBaseCommand;

public class AutoFillScheduleCommandHandler : IRequestHandler<AutoFillScheduleCommand, List<ScheduleBlockDto>>
{
    private const int WorkDayStartHour = 8;  // 8 AM UTC
    private const int WorkDayEndHour = 22;   // 10 PM UTC
    private const int MinGapMinutes = 15;

    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;
    private readonly ISchedulerService _scheduler;

    public AutoFillScheduleCommandHandler(
        IUnitOfWork uow,
        ICurrentUserService currentUser,
        ISchedulerService scheduler)
    {
        _uow = uow;
        _currentUser = currentUser;
        _scheduler = scheduler;
    }

    public async Task<List<ScheduleBlockDto>> Handle(AutoFillScheduleCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId;
        var dayStart = request.Date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var dayEnd = dayStart.AddDays(1);
        var workStart = dayStart.AddHours(WorkDayStartHour);
        var workEnd = dayStart.AddHours(WorkDayEndHour);

        // Get existing active blocks for the day
        var existingBlocks = await _uow.ScheduleBlocks.GetByDateRangeAsync(userId, dayStart, dayEnd, ct);
        var activeBlocks = existingBlocks
            .Where(b => b.Status != BlockStatus.Skipped && b.Status != BlockStatus.Completed)
            .OrderBy(b => b.StartTime)
            .ToList();

        // Get pending tasks not yet scheduled today
        var tasks = await _scheduler.GetUnscheduledTasksAsync(userId, dayStart, dayEnd, ct);

        if (tasks.Count == 0)
            return [];

        // Find free gaps in the workday
        var gaps = FindGaps(workStart, workEnd, activeBlocks);

        var created = new List<ScheduleBlock>();
        var taskIndex = 0;

        foreach (var (gapStart, gapEnd) in gaps)
        {
            var cursor = gapStart;

            while (taskIndex < tasks.Count && cursor < gapEnd)
            {
                var task = tasks[taskIndex];
                var duration = task.EstimatedMinutes!.Value;
                var blockEnd = cursor.AddMinutes(duration);

                if (blockEnd > gapEnd)
                    break; // task doesn't fit in remaining gap

                var block = new ScheduleBlock
                {
                    UserId = userId,
                    Title = task.Title,
                    StartTime = cursor,
                    EndTime = blockEnd,
                    BlockType = BlockType.Flexible,
                    Status = BlockStatus.Scheduled,
                    ProjectId = task.ProjectId,
                    TaskId = task.TaskId
                };

                await _uow.ScheduleBlocks.AddAsync(block, ct);
                created.Add(block);
                cursor = blockEnd;
                taskIndex++;
            }

            if (taskIndex >= tasks.Count)
                break;
        }

        if (created.Count > 0)
            await _uow.SaveChangesAsync(ct);

        return created.Select(ScheduleBlockDto.FromEntity).ToList();
    }

    private static List<(DateTime Start, DateTime End)> FindGaps(
        DateTime workStart, DateTime workEnd,
        List<ScheduleBlock> activeBlocks)
    {
        var gaps = new List<(DateTime, DateTime)>();
        var cursor = workStart;

        foreach (var block in activeBlocks)
        {
            var blockStart = block.StartTime < workStart ? workStart : block.StartTime;
            var blockEnd = block.EndTime > workEnd ? workEnd : block.EndTime;

            if (blockStart > cursor)
            {
                var gapMinutes = (blockStart - cursor).TotalMinutes;
                if (gapMinutes >= MinGapMinutes)
                    gaps.Add((cursor, blockStart));
            }

            if (blockEnd > cursor)
                cursor = blockEnd;
        }

        if (cursor < workEnd)
        {
            var remainingMinutes = (workEnd - cursor).TotalMinutes;
            if (remainingMinutes >= MinGapMinutes)
                gaps.Add((cursor, workEnd));
        }

        return gaps;
    }
}
