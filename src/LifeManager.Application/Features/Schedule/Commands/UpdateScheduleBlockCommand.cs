using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.Schedule.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Schedule.Commands;

public record UpdateScheduleBlockCommand(
    Guid Id,
    string Title,
    string? Notes,
    Guid? ProjectId,
    Guid? TaskId) : IRequest<ScheduleBlockDto>, IBaseCommand;

public class UpdateScheduleBlockCommandHandler : IRequestHandler<UpdateScheduleBlockCommand, ScheduleBlockDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateScheduleBlockCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ScheduleBlockDto> Handle(UpdateScheduleBlockCommand request, CancellationToken ct)
    {
        var block = await _uow.ScheduleBlocks.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("ScheduleBlock", request.Id);

        block.Title = request.Title;
        block.Notes = request.Notes;
        block.ProjectId = request.ProjectId;
        block.TaskId = request.TaskId;

        _uow.ScheduleBlocks.Update(block);
        await _uow.SaveChangesAsync(ct);

        return ScheduleBlockDto.FromEntity(block);
    }
}
