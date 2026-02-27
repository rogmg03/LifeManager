using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Enums;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Schedule.Commands;

public record DeleteScheduleBlockCommand(Guid Id) : IRequest, IBaseCommand;

public class DeleteScheduleBlockCommandHandler : IRequestHandler<DeleteScheduleBlockCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteScheduleBlockCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteScheduleBlockCommand request, CancellationToken ct)
    {
        var block = await _uow.ScheduleBlocks.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("ScheduleBlock", request.Id);

        if (block.BlockType != BlockType.Flexible)
            throw new ConflictException("Only Flexible blocks can be deleted.");

        _uow.ScheduleBlocks.Delete(block);
        await _uow.SaveChangesAsync(ct);
    }
}
