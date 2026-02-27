using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Schedule.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Schedule.Commands;

public record CreateScheduleBlockCommand(
    string Title,
    DateTime StartTime,
    DateTime EndTime,
    BlockType BlockType,
    string? Notes,
    Guid? ProjectId,
    Guid? TaskId) : IRequest<ScheduleBlockDto>, IBaseCommand;

public class CreateScheduleBlockCommandHandler : IRequestHandler<CreateScheduleBlockCommand, ScheduleBlockDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CreateScheduleBlockCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    { _uow = uow; _currentUser = currentUser; }

    public async Task<ScheduleBlockDto> Handle(CreateScheduleBlockCommand request, CancellationToken ct)
    {
        var block = new ScheduleBlock
        {
            UserId = _currentUser.UserId,
            Title = request.Title,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            BlockType = request.BlockType,
            Notes = request.Notes,
            ProjectId = request.ProjectId,
            TaskId = request.TaskId
        };

        await _uow.ScheduleBlocks.AddAsync(block, ct);
        await _uow.SaveChangesAsync(ct);

        return ScheduleBlockDto.FromEntity(block);
    }
}
