using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Exceptions;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.CollegeCourseDetails.Commands;

public record DeleteCollegeCourseDetailCommand(Guid ProjectId) : IRequest, IBaseCommand;

public class DeleteCollegeCourseDetailCommandHandler : IRequestHandler<DeleteCollegeCourseDetailCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteCollegeCourseDetailCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteCollegeCourseDetailCommand request, CancellationToken ct)
    {
        var detail = await _uow.CollegeCourseDetails.GetByProjectIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("CollegeCourseDetail", request.ProjectId);

        _uow.CollegeCourseDetails.Delete(detail);
        await _uow.SaveChangesAsync(ct);
    }
}
