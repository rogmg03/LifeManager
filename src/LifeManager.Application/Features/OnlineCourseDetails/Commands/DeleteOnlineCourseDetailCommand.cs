using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.OnlineCourseDetails.Commands;

public record DeleteOnlineCourseDetailCommand(Guid ProjectId) : IRequest, IBaseCommand;

public class DeleteOnlineCourseDetailCommandHandler : IRequestHandler<DeleteOnlineCourseDetailCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteOnlineCourseDetailCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteOnlineCourseDetailCommand request, CancellationToken ct)
    {
        var detail = await _uow.OnlineCourseDetails.GetByProjectIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("OnlineCourseDetail", request.ProjectId);

        _uow.OnlineCourseDetails.Delete(detail);
        await _uow.SaveChangesAsync(ct);
    }
}
