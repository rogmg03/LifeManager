using LifeManager.Application.Features.OnlineCourseDetails.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.OnlineCourseDetails.Queries;

public record GetOnlineCourseDetailQuery(Guid ProjectId) : IRequest<OnlineCourseDetailDto?>;

public class GetOnlineCourseDetailQueryHandler : IRequestHandler<GetOnlineCourseDetailQuery, OnlineCourseDetailDto?>
{
    private readonly IUnitOfWork _uow;
    public GetOnlineCourseDetailQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<OnlineCourseDetailDto?> Handle(GetOnlineCourseDetailQuery request, CancellationToken ct)
    {
        var detail = await _uow.OnlineCourseDetails.GetByProjectIdAsync(request.ProjectId, ct);
        return detail is null ? null : OnlineCourseDetailDto.FromEntity(detail);
    }
}
