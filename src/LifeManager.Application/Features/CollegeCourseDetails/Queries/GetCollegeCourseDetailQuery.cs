using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Features.CollegeCourseDetails.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.CollegeCourseDetails.Queries;

public record GetCollegeCourseDetailQuery(Guid ProjectId) : IRequest<CollegeCourseDetailDto>;

public class GetCollegeCourseDetailQueryHandler : IRequestHandler<GetCollegeCourseDetailQuery, CollegeCourseDetailDto>
{
    private readonly IUnitOfWork _uow;
    public GetCollegeCourseDetailQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CollegeCourseDetailDto> Handle(GetCollegeCourseDetailQuery request, CancellationToken ct)
    {
        var detail = await _uow.CollegeCourseDetails.GetByProjectIdAsync(request.ProjectId, ct)
            ?? throw new NotFoundException("CollegeCourseDetail", request.ProjectId);
        return CollegeCourseDetailDto.FromEntity(detail);
    }
}
